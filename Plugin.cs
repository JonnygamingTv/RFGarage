using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RFGarage.DatabaseManagers;
using RFGarage.Enums;
using RFGarage.EventListeners;
using RFGarage.Models;
using RFRocketLibrary;
using RFRocketLibrary.Enum;
using RFRocketLibrary.Events;
using RFRocketLibrary.Utils;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using RocketExtensions.Plugins;
using RocketExtensions.Utilities;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFGarage
{
    public class Plugin : ExtendedRocketPlugin<Configuration>
    {
        private static int Major = 1;
        private static int Minor = 1;
        private static int Patch = 4;

        public static Plugin Inst;
        public static Configuration Conf;
        internal static Color MsgColor;
        internal Dictionary<ulong, DateTime?> IsProcessingGarage;
        internal HashSet<uint> BusyVehicle;
        internal Dictionary<Rocket.Unturned.Player.UnturnedPlayer, List<InteractableVehicle>> vehicleQueue = new Dictionary<Rocket.Unturned.Player.UnturnedPlayer, List<InteractableVehicle>>();

        protected override void Load()
        {
            Inst = this;
            Conf = Configuration.Instance;

            if (Conf.Enabled)
            {
                MsgColor = UnturnedChat.GetColorFromName(Conf.MessageColor, Color.green);

                DependencyUtil.Load(EDependency.NewtonsoftJson);
                DependencyUtil.Load(EDependency.SystemRuntimeSerialization);
                DependencyUtil.Load(EDependency.LiteDB);
                DependencyUtil.Load(EDependency.LiteDBAsync);
                DependencyUtil.Load(EDependency.Dapper);
                DependencyUtil.Load(EDependency.I18N);
                DependencyUtil.Load(EDependency.I18NWest);
                DependencyUtil.Load(EDependency.MySqlData);
                DependencyUtil.Load(EDependency.SystemManagement);
                DependencyUtil.Load(EDependency.UbietyDnsCore);
                DependencyUtil.Load(EDependency.ZstdNet);

                DatabaseManager.Initialize();
                GarageManager.Initialize();
                
                Library.AttachEvent(true);
                Level.onPostLevelLoaded += ServerEvent.OnPostLevelLoaded;
                UnturnedEvent.OnVehicleExploded += VehicleEvent.OnExploded;
                if (Conf.AutoAddOnDrown)
                    UnturnedPatchEvent.OnPreVehicleDestroyed += VehicleEvent.OnPreVehicleDestroyed;
                if (Conf.AutoGarageOnLeave != -1)
                    if (Conf.AutoGarageOnLeave == 0f)
                    {
                        Rocket.Unturned.U.Events.OnPlayerDisconnected += DisConnA;
                    }
                    else
                    {
                        Rocket.Unturned.U.Events.OnPlayerDisconnected += DisConnB;
                        Rocket.Unturned.U.Events.OnPlayerConnected += PConn;
                    }
                        

                if (Level.isLoaded)
                    ServerEvent.OnPostLevelLoaded(0);
            }
            else
                Logger.LogError($"[{Name}] Plugin: DISABLED");

            Logger.LogWarning($"[{Name}] Plugin loaded successfully!");
            Logger.LogWarning($"[{Name}] {Name} v{Major}.{Minor}.{Patch}");
            Logger.LogWarning($"[{Name}] Made with 'rice' by RiceField Plugins!");
        }

        protected override void Unload()
        {
            if (Conf.Enabled)
            {
                StopAllCoroutines();

                Level.onPostLevelLoaded -= ServerEvent.OnPostLevelLoaded;
                UnturnedEvent.OnVehicleExploded -= VehicleEvent.OnExploded;
                if (Conf.AutoAddOnDrown)
                    UnturnedPatchEvent.OnPreVehicleDestroyed -= VehicleEvent.OnPreVehicleDestroyed;
                if (Conf.AutoGarageOnLeave != -1)
                    if (Conf.AutoGarageOnLeave == 0)
                    {
                        Rocket.Unturned.U.Events.OnPlayerDisconnected -= DisConnA;
                    }
                    else
                    {
                        Rocket.Unturned.U.Events.OnPlayerDisconnected -= DisConnB;
                        Rocket.Unturned.U.Events.OnPlayerConnected -= PConn;
                    }

                Library.DetachEvent(true);
#if RF
                Library.Uninitialize();
                #endif
            }

            Conf = null;
            Inst = null;

            Logger.LogWarning($"[{Name}] Plugin unloaded successfully!");
        }
        private void DisConnA(Rocket.Unturned.Player.UnturnedPlayer player) {
            for(int veh = 0; veh < VehicleManager.vehicles.Count; veh++)
            {
                InteractableVehicle vehicle = VehicleManager.vehicles[veh];
                if(!vehicle.isDead && !vehicle.isExploded && vehicle.isLocked && vehicle.lockedOwner.m_SteamID == player.CSteamID.m_SteamID && (RFGarage.Plugin.Conf.AllowTrain || vehicle.asset.engine != EEngine.TRAIN) && !Conf.Blacklists.Any(x => x.Type == EBlacklistType.VEHICLE && !player.HasPermission(x.BypassPermission) && x.IdList.Contains(vehicle.id)))
                {
                    _ = ToGarage(player, vehicle, vehicle.asset.vehicleName);
                }
            }
        }
        private void DisConnB(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            List<InteractableVehicle> vehicles = new List<InteractableVehicle>();
            for (int veh = 0; veh < VehicleManager.vehicles.Count; veh++)
            {
                InteractableVehicle vehicle = VehicleManager.vehicles[veh];
                if (!vehicle.isDead && !vehicle.isExploded && vehicle.isLocked && vehicle.lockedOwner.m_SteamID == player.CSteamID.m_SteamID && (RFGarage.Plugin.Conf.AllowTrain || vehicle.asset.engine != EEngine.TRAIN) && !Conf.Blacklists.Any(x => x.Type == EBlacklistType.VEHICLE && !player.HasPermission(x.BypassPermission) && x.IdList.Contains(vehicle.id)))
                {
                    vehicles.Add(vehicle);
                }
            }
            if (vehicleQueue.ContainsKey(player)) vehicleQueue.Remove(player);
            vehicleQueue.Add(player, vehicles);
            StartCoroutine(ToGarageSoon(player));
        }
        private void PConn(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            vehicleQueue.Remove(player);
            StopCoroutine(ToGarageSoon(player));
        }
        private IEnumerator ToGarageSoon(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            yield return new WaitForSeconds(Conf.AutoGarageOnLeave);
            for (byte i = 0; i < vehicleQueue[player].Count; i++) if (vehicleQueue[player][i] && !vehicleQueue[player][i].isDead && vehicleQueue[player][i].lockedOwner.m_SteamID == player.CSteamID.m_SteamID) _ = ToGarage(player, vehicleQueue[player][i], vehicleQueue[player][i].asset.vehicleName);
            vehicleQueue.Remove(player);
            yield break;
        }
        public async System.Threading.Tasks.Task ToGarage(Rocket.Unturned.Player.UnturnedPlayer player, InteractableVehicle vehicle, string vehicleName)
        {
            foreach (var blacklist in RFGarage.Plugin.Conf.Blacklists.Where(x => x.Type == EBlacklistType.BARRICADE))
            {
                if (player.HasPermission(blacklist.BypassPermission))
                    continue;

                var region = BarricadeManager.getRegionFromVehicle(vehicle);
                if (region == null)
                    continue;

                foreach (var drop in region.drops.Where(drop => blacklist.IdList.Contains(drop.asset.id)))
                {
                    return;
                }
            }

            foreach (var blacklist in RFGarage.Plugin.Conf.Blacklists.Where(x => x.Type == EBlacklistType.ITEM))
            {
                if (player.HasPermission(blacklist.BypassPermission))
                    continue;

                var region = BarricadeManager.getRegionFromVehicle(vehicle);
                if (region == null)
                    continue;

                foreach (var drop in region.drops)
                {
                    if (drop.interactable is not InteractableStorage storage)
                        continue;
                    foreach (var asset in from id in blacklist.IdList
                                          where storage.items.has(id) != null
                                          select AssetUtil.GetItemAsset(id))
                    {
                        return;
                    }
                }
            }
            if (vehicle.trunkItems != null && vehicle.trunkItems.getItemCount() != 0)
            {
                foreach (var blacklist in RFGarage.Plugin.Conf.Blacklists.Where(x => x.Type == EBlacklistType.ITEM))
                {
                    if (player.HasPermission(blacklist.BypassPermission))
                        continue;

                    foreach (var asset in from itemJar in vehicle.trunkItems.items
                                          where blacklist.IdList.Contains(itemJar.item.id)
                                          select AssetUtil.GetItemAsset(itemJar.item.id))
                    {
                        return;
                    }
                }
            }
            var garageContent = RFRocketLibrary.Models.VehicleWrapper.Create(vehicle);
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                vehicle.forceRemoveAllPlayers();
                RFGarage.Utils.VehicleUtil.ClearTrunkAndBarricades(vehicle);
                VehicleManager.askVehicleDestroy(vehicle);
            });
            await DatabaseManager.Queue.Enqueue(async () =>
                await GarageManager.AddAsync(new PlayerGarage
                {
                    SteamId = player.CSteamID.m_SteamID,
                    VehicleName = vehicleName,
                    GarageContent = garageContent,
                    LastUpdated = DateTime.Now,
                })
            )!;
        }

        public override TranslationList DefaultTranslations => new()
        {
            {$"{EResponse.INVALID_PARAMETER}", "Invalid parameter! Usage: {0}"},
            {$"{EResponse.SAME_DATABASE}", "You can't migrate to the same database!"},
            {$"{EResponse.DATABASE_NOT_READY}", "Database is not ready, please wait..."},
            {$"{EResponse.MIGRATION_START}", "Starting migration from {0} to {1}..."},
            {$"{EResponse.MIGRATION_FINISH}", "Migration finished 100%!"},
            {$"{EResponse.NO_VEHICLE_INPUT}", "You are not looking or seating at any vehicle!"},
            {$"{EResponse.VEHICLE_NOT_OWNED}", "You don't own this vehicle!"},
            {$"{EResponse.GARAGE_FULL}", "Your garage slot is full! Max slot: {0}"},
            {$"{EResponse.TRAIN_NOT_ALLOWED}", "Train is not allowed!"},
            {$"{EResponse.BLACKLIST_VEHICLE}", "{0} ({1}) is blacklisted!"},
            {$"{EResponse.BLACKLIST_BARRICADE}", "This vehicle has blacklisted barricade! {0} ({1})"},
            {$"{EResponse.BLACKLIST_ITEM}", "This vehicle has blacklisted item! {0} ({1})"},
            {$"{EResponse.VEHICLE_NOT_FOUND}", "You don't have {0} inside your garage!"},
            {$"{EResponse.GARAGE_RETRIEVE}", "Successfully retrieved your {0} from garage!"},
            {$"{EResponse.GARAGE_ADDED}", "Successfully added {0} to garage!"},
            {$"{EResponse.NO_VEHICLE}", "You don't have any vehicle in garage!"},
            {$"{EResponse.GARAGE_SLOT}", "Current garage slot: {0}/{1}"},
            {$"{EResponse.GARAGE_LIST}", "#{0} {1} [Vehicle ID: {2} Vehicle Name: {3}]"},
            {$"{EResponse.VEHICLE_DROWN}", "Your drowned {0} has been added to your garage automatically!"},
            {
                $"{EResponse.PROCESSING_GARAGE}",
                "Please wait! We are still processing your previous garage request!"
            },
        };
    }
}
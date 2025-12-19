using System;
using System.Linq;
using RFRocketLibrary.Utils;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Action = SDG.Unturned.Action;

// ReSharper disable InconsistentNaming

namespace RFRocketLibrary.Events
{
    public static class UnturnedEvent
    {
        #region Delegates

        // Delegates
        public delegate void PlayerBled(Player player);

        public delegate void PlayerBrokeBone(Player player);

        public delegate void PlayerCaughtFish(Player player);

        public delegate void PlayerChangedEquipment(Player player);

        public delegate void PlayerChangedGesture(Player player, EPlayerGesture gesture);

        public delegate void PlayerChatted(Player player, EChatMode mode, ref Color chatted, ref bool isRich,
            string text, ref bool isVisible);

        public delegate void PlayerChoppedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin);

        public delegate void PlayerExperienceChanged(Player player, uint oldExperience, uint newExperience);

        public delegate void PlayerInspectedEquipment(Player player);

        public delegate void PlayerMinedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin);

        public delegate void PlayerPunched(Player player, EPlayerPunch punch);

        public delegate void PlayerReloadedGun(UseableGun gun);

        public delegate void PlayerReputationChanged(Player player, int oldReputation, int newReputation);

        public delegate void PlayerSkillUpgraded(Player player, byte speciality, byte index, byte level);

        public delegate void PlayerTookItem(Player player);

        public delegate void PlayerWonArena(Player player);

        public delegate void PrePlayerChoppedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin, ref bool shouldAllow);

        public delegate void PrePlayerJoined(SteamPending? steamPending, ref bool shouldAllow);

        public delegate void PrePlayerMinedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin, ref bool shouldAllow);

        public delegate void ResourceDamaged(CSteamID cSteamID, ResourceSpawnpoint resourceSpawnpoint, ushort damage,
            EDamageOrigin damageOrigin);

        public delegate void VehicleDestroyed(InteractableVehicle vehicle);

        public delegate void VehicleExploded(InteractableVehicle vehicle);

        #endregion

        #region Events

        public static event RepairedBarricadeHandler? OnBarricadeRepaired;
        public static event BarricadeSpawnedHandler? OnBarricadeSpawned;
        public static event UseableConsumeable.PerformedAidHandler? OnPlayerAided;
        public static event PlayerBled? OnPlayerBled;
        public static event PlayerBrokeBone? OnPlayerBrokeBone;
        public static event PlayerCaughtFish? OnPlayerCaughtFish;
        public static event EffectManager.EffectTextCommittedHandler? OnPlayerChangedEffectInputField;
        public static event PlayerChangedEquipment? OnPlayerChangedEquipment;
        public static event PlayerChangedGesture? OnPlayerChangedGesture;
        public static event PlayerChatted? OnPlayerChatted;
        public static event PlayerChoppedResource? OnPlayerChoppedResource;
        public static event EffectManager.EffectButtonClickedHandler? OnPlayerClickedEffectButton;
        public static event UseableConsumeable.ConsumePerformedHandler? OnPlayerConsumedItem;
        public static event PlayerLife.PlayerDiedCallback? OnPlayerDied;
        public static event PlayerExperienceChanged? OnPlayerExperienceChanged;
        public static event UseableGun.BulletSpawnedHandler? OnPlayerGunFired;
        public static event PlayerInspectedEquipment? OnPlayerInspectedEquipment;
        public static event PlayerLife.PlayerDiedCallback? OnPlayerKilled;
        public static event PlayerMinedResource? OnPlayerMinedResource;
        public static event PlayerPunched? OnPlayerPunched;
        public static event PlayerReloadedGun? OnPlayerReloadedGun;
        public static event PlayerReputationChanged? OnPlayerReputationChanged;
        public static event PlayerSkillUpgraded? OnPlayerSkillUpgraded;
        public static event UseableThrowable.ThrowableSpawnedHandler? OnPlayerThrewThrowable;
        public static event PlayerTookItem? OnPlayerTookItem;
        public static event PlayerWonArena? OnPlayerWonArena;
        public static event DamageTool.DamageAnimalHandler? OnPreAnimalDamaged;
        public static event DamageBarricadeRequestHandler? OnPreBarricadeDamaged;
        public static event RepairBarricadeRequestHandler? OnPreBarricadeRepaired;
        public static event BarricadeDrop.SalvageRequestHandler? OnPreBarricadeSalvaged;
        public static event DeployBarricadeRequestHandler? OnPreBarricadeSpawned;
        public static event TransformBarricadeRequestHandler? OnPreBarricadeTransformed;
        public static event ServerSpawningItemDropHandler? OnPreItemSpawned;
        public static event DamageObjectRequestHandler? OnPreObjectDamaged;
        public static event InteractableFarm.HarvestRequestHandler? OnPrePlayerInteractedFarm;
        public static event UseableConsumeable.PerformingAidHandler? OnPrePlayerAided;
        public static event VehicleCarjackedSignature? OnPrePlayerCarjackedVehicle;
        public static event PrePlayerChoppedResource? OnPrePlayerChoppedResource;
        public static event UseableConsumeable.ConsumeRequestedHandler? OnPrePlayerConsumedItem;
        public static event PlayerCraftingRequestHandler? OnPrePlayerCraftedItem;
        public static event DamageTool.DamagePlayerHandler? OnPrePlayerDamaged;
        public static event Action<PlayerLife>? OnPrePlayerDied;
        public static event UseableGun.BulletHitHandler? OnPrePlayerGunHitEntity;
        public static event PrePlayerJoined? OnPrePlayerJoined;
        public static event VehicleLockpickedSignature? OnPrePlayerLockpickedVehicle;
        public static event PrePlayerMinedResource? OnPrePlayerMinedResource;
        public static event PlayerLife.RespawnPointSelector? OnPrePlayerRespawned;
        public static event TakeItemRequestHandler? OnPrePlayerTookItem;
        public static event DamageResourceRequestHandler? OnPreResourceDamaged;
        public static event ModifySignRequestHandler? OnPreSignModified;
        public static event OpenStorageRequestHandler? OnPreStorageOpened;
        public static event DamageStructureRequestHandler? OnPreStructureDamaged;
        public static event RepairStructureRequestHandler? OnPreStructureRepaired;
        public static event StructureDrop.SalvageRequestHandler? OnPreStructureSalvaged;
        public static event DeployStructureRequestHandler? OnPreStructureSpawned;
        public static event TransformStructureRequestHandler? OnPreStructureTransformed;
        public static event DamageTool.DamageZombieHandler? OnPreZombieDamaged;
        public static event Action<Player, InteractableObject>? OnQuestObjectUsed;
        public static event ResourceDamaged? OnResourceDamaged;
        public static event ServerSendingChatMessageHandler? OnServerMessageSent;
        public static event RepairedStructureHandler? OnStructureRepaired;
        public static event StructureSpawnedHandler? OnStructureSpawned;
        public static event VehicleDestroyed? OnVehicleDestroyed;
        public static event VehicleExploded? OnVehicleExploded;
        public static event VehicleManager.EnterVehicleRequestHandler? OnPrePlayerEnteredVehicle;
        public static event VehicleManager.ExitVehicleRequestHandler? OnPrePlayerExitedVehicle;
        public static event DamageVehicleRequestHandler? OnPreVehicleDamaged;
        public static event DamageTireRequestHandler? OnPreVehicleTireDamaged;
        public static event Action<PlayerStance>? OnPlayerChangedStance;
        public static event Action<PlayerAnimator>? OnPlayerChangedLean;
        public static event Action<PlayerLife>? OnPlayerRevived;
        public static event Action<InteractableVehicle>? OnVehicleFuelUpdated;
        public static event Action<InteractableVehicle>? OnVehicleHealthUpdated;
        public static event Action<InteractableVehicle>? OnVehicleBatteryUpdated;
        public static event Action<InteractableVehicle>? OnVehicleLockUpdated;
        public static event Action<InteractableVehicle, int>? OnVehicleAddedPassenger;
        public static event Action<InteractableVehicle, int, int>? OnVehiclePassengerChangedSeat;
        public static event Action<InteractableVehicle, int, Player>? OnVehicleRemovedPassenger;
        public static event Action<PlayerLife>? OnPlayerHealthUpdated;
        public static event Action<PlayerLife>? OnPlayerFoodUpdated;
        public static event Action<PlayerLife>? OnPlayerWaterUpdated;
        public static event Action<PlayerLife>? OnPlayerVirusUpdated;
        public static event DamageTool.PlayerAllowedToDamagePlayerHandler? OnPrePlayerAllowedToDamagePlayer;
        public static event RepairVehicleRequestHandler? OnPreVehicleRepaired;
        public static event SiphonVehicleRequestHandler? OnPrePlayerSiphonedVehicle;
        public static event VehicleManager.SwapSeatRequestHandler? OnPrePlayerSwappedVehicleSeat;
        public static event VehicleManager.ToggleVehicleLockRequested? OnPreVehicleLockUpdated;
        public static event Action<PlayerClothing>? OnPlayerChangedBackpack;
        public static event Action<PlayerClothing>? OnPlayerChangedGlasses;
        public static event Action<PlayerClothing>? OnPlayerChangedHat;
        public static event Action<PlayerClothing>? OnPlayerChangedMask;
        public static event Action<PlayerClothing>? OnPlayerChangedPants;
        public static event Action<PlayerClothing>? OnPlayerChangedShirt;
        public static event Action<PlayerClothing>? OnPlayerChangedVest;
        public static event PluginKeyTickHandler? OnPlayerPluginKeyUpdated;
        public static event UseableTire.ModifyTireRequestHandler? OnPrePlayerModifiedTire;

        #endregion

        internal static void OnPrePlayerAllowedToDamagePlayerInvoker(Player instigator, Player victim, ref bool isallowed)
        {
            OnPrePlayerAllowedToDamagePlayer?.Invoke(instigator, victim, ref isallowed);
        }
        
        internal static void OnBarricadeRepairedInvoker(CSteamID instigatorSteamId, Transform barricadeTransform,
            float totalHealing)
        {
            OnBarricadeRepaired?.Invoke(instigatorSteamId, barricadeTransform, totalHealing);
        }

        internal static void OnPreBarricadeDamagedInvoker(CSteamID instigatorsteamid, Transform barricadetransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageorigin)
        {
            OnPreBarricadeDamaged?.Invoke(instigatorsteamid, barricadetransform, ref pendingTotalDamage, ref shouldAllow, damageorigin);
        }

        internal static void OnPreBarricadeRepairedInvoker(CSteamID instigatorSteamId, Transform barricadeTransform,
            ref float pendingTotalHealing, ref bool shouldAllow)
        {
            OnPreBarricadeRepaired?.Invoke(instigatorSteamId, barricadeTransform, ref pendingTotalHealing,
                ref shouldAllow);
        }

        internal static void OnPreBarricadeTransformedInvoker(CSteamID instigator, byte x, byte y, ushort plant,
            uint instanceId, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z,
            ref bool shouldAllow)
        {
            OnPreBarricadeTransformed?.Invoke(instigator, x, y, plant, instanceId, ref point, ref angle_x, ref angle_y,
                ref angle_z, ref shouldAllow);
        }

        internal static void OnPreObjectDamagedInvoker(CSteamID instigatorSteamId, Transform objectTransform,
            byte section, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            OnPreObjectDamaged?.Invoke(instigatorSteamId, objectTransform, section, ref pendingTotalDamage,
                ref shouldAllow, damageOrigin);
        }

        internal static void OnPrePlayerRespawnedInvoker(PlayerLife sender, bool wantsToSpawnAtHome, ref Vector3 position,
            ref float yaw)
        {
            OnPrePlayerRespawned?.Invoke(sender, wantsToSpawnAtHome, ref position, ref yaw);
        }

        internal static void OnPreSignModifiedInvoker(CSteamID instigator, InteractableSign sign, ref string text, ref bool shouldallow)
        {
            OnPreSignModified?.Invoke(instigator, sign, ref text, ref shouldallow);
        }

        internal static void OnPreStorageOpenedInvoker(CSteamID instigator, InteractableStorage storage, ref bool shouldallow)
        {
            OnPreStorageOpened?.Invoke(instigator, storage, ref shouldallow);
        }

        internal static void OnPreStructureDamagedInvoker(CSteamID instigatorsteamid, Transform structuretransform, ref ushort pendingtotaldamage, ref bool shouldallow, EDamageOrigin damageorigin)
        {
            OnPreStructureDamaged?.Invoke(instigatorsteamid, structuretransform, ref pendingtotaldamage, ref shouldallow, damageorigin);
        }

        internal static void OnPreStructureRepairedInvoker(CSteamID instigatorSteamId, Transform structureTransform,
            ref float pendingTotalHealing, ref bool shouldAllow)
        {
            OnPreStructureRepaired?.Invoke(instigatorSteamId, structureTransform, ref pendingTotalHealing,
                ref shouldAllow);
        }

        internal static void OnPreStructureTransformedInvoker(CSteamID instigator, byte x, byte y, uint instanceId,
            ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow)
        {
            OnPreStructureTransformed?.Invoke(instigator, x, y, instanceId, ref point, ref angle_x, ref angle_y,
                ref angle_z, ref shouldAllow);
        }

        internal static void OnQuestObjectUsedInvoker(Player player, InteractableObject @object)
        {
            OnQuestObjectUsed?.Invoke(player, @object);
        }

        internal static void OnStructureRepairedInvoker(CSteamID instigatorSteamId, Transform structureTransform,
            float totalHealing)
        {
            OnStructureRepaired?.Invoke(instigatorSteamId, structureTransform, totalHealing);
        }

        // Invokers
        internal static void OnPreAnimalDamagedInvoker(ref DamageAnimalParameters parameters, ref bool shouldAllow)
        {
            OnPreAnimalDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPreBarricadeSalvagedInvoker(BarricadeDrop barricade, SteamPlayer instigatorClient,
            ref bool shouldAllow)
        {
            OnPreBarricadeSalvaged?.Invoke(barricade, instigatorClient, ref shouldAllow);
        }

        internal static void OnBarricadeSpawnedInvoker(BarricadeRegion region, BarricadeDrop drop)
        {
            OnBarricadeSpawned?.Invoke(region, drop);
        }

        internal static void OnPreBarricadeSpawnedInvoker(Barricade barricade, ItemBarricadeAsset asset, Transform hit,
            ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner,
            ref ulong @group, ref bool shouldAllow)
        {
            OnPreBarricadeSpawned?.Invoke(barricade, asset, hit, ref point, ref angle_x, ref angle_y, ref angle_z,
                ref owner, ref group, ref shouldAllow);
        }

        internal static void OnPlayerEffectButtonClickedInvoker(Player player, string buttonName)
        {
            OnPlayerClickedEffectButton?.Invoke(player, buttonName);
        }

        internal static void OnPlayerEffectInputFieldChangedInvoker(Player player, string buttonName, string text)
        {
            OnPlayerChangedEffectInputField?.Invoke(player, buttonName, text);
        }

        internal static void OnPlayerItemConsumedInvoker(Player instigatingPlayer,
            ItemConsumeableAsset consumeableAsset)
        {
            OnPlayerConsumedItem?.Invoke(instigatingPlayer, consumeableAsset);
        }

        internal static void OnPrePlayerItemConsumedInvoker(Player instigatingPlayer,
            ItemConsumeableAsset consumeableAsset,
            ref bool shouldAllow)
        {
            OnPrePlayerConsumedItem?.Invoke(instigatingPlayer, consumeableAsset, ref shouldAllow);
        }

        internal static void OnPrePlayerItemCraftedInvoker(PlayerCrafting crafting, ref ushort itemId,
            ref byte blueprintIndex,
            ref bool shouldAllow)
        {
            OnPrePlayerCraftedItem?.Invoke(crafting, ref itemId, ref blueprintIndex, ref shouldAllow);
        }

        internal static void OnPreItemSpawnedInvoker(Item item, ref Vector3 location, ref bool shouldAllow)
        {
            OnPreItemSpawned?.Invoke(item, ref location, ref shouldAllow);
        }

        internal static void OnPreItemTakenInvoker(Player player, byte x, byte y, uint instanceId, byte to_x, byte to_y,
            byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            OnPrePlayerTookItem?.Invoke(player, x, y, instanceId, to_x, to_y, to_rot, to_page, itemData,
                ref shouldAllow);
        }

        internal static void OnPrePlayerInteractedFarmInvoker(InteractableFarm harvestable, SteamPlayer instigatorPlayer,
            ref bool shouldAllow)
        {
            if (instigatorPlayer == null)
                return;
            
            OnPrePlayerInteractedFarm?.Invoke(harvestable, instigatorPlayer, ref shouldAllow);
        }

        internal static void OnPlayerAidedInvoker(Player instigator, Player target)
        {
            OnPlayerAided?.Invoke(instigator, target);
        }

        internal static void OnPrePlayerAidedInvoker(Player instigator, Player target, ItemConsumeableAsset asset,
            ref bool shouldAllow)
        {
            OnPrePlayerAided?.Invoke(instigator, target, asset, ref shouldAllow);
        }

        internal static void OnPlayerChattedInvoker(SteamPlayer player, EChatMode mode, ref Color chatted,
            ref bool isRich, string text, ref bool isVisible)
        {
            OnPlayerChatted?.Invoke(player.player, mode, ref chatted, ref isRich, text, ref isVisible);
        }

        internal static void OnPrePlayerDamagedInvoker(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            OnPrePlayerDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPrePlayerDiedInvoker(PlayerLife life)
        {
            OnPrePlayerDied?.Invoke(life);
        }

        internal static void OnPrePlayerJoinedInvoker(ValidateAuthTicketResponse_t callback, ref bool isValid,
            ref string explanation)
        {
            OnPrePlayerJoined?.Invoke(callback.m_SteamID.GetSteamPending(), ref isValid);
        }

        internal static void OnPlayerKilledInvoker(PlayerLife sender, EDeathCause cause, ELimb limb,
            CSteamID instigator)
        {
            OnPlayerKilled?.Invoke(sender, cause, limb, instigator);
        }

        internal static void OnPlayerStatIncrementedInvoker(Player player, EPlayerStat stat)
        {
            switch (stat)
            {
                case EPlayerStat.ARENA_WINS:
                    OnPlayerWonArena?.Invoke(player);
                    break;
                case EPlayerStat.FOUND_FISHES:
                    OnPlayerCaughtFish?.Invoke(player);
                    break;
                case EPlayerStat.FOUND_ITEMS:
                    OnPlayerTookItem?.Invoke(player);
                    break;
            }
        }

        internal static void OnServerSentMessageInvoker(ref string text, ref Color color, SteamPlayer fromPlayer,
            SteamPlayer toPlayer, EChatMode mode, ref string iconURL, ref bool useRichTextFormatting)
        {
            OnServerMessageSent?.Invoke(ref text, ref color, fromPlayer, toPlayer, mode, ref iconURL,
                ref useRichTextFormatting);
        }

        internal static void OnPreStructureSalvagedInvoker(StructureDrop structure, SteamPlayer instigatorClient,
            ref bool shouldAllow)
        {
            OnPreStructureSalvaged?.Invoke(structure, instigatorClient, ref shouldAllow);
        }

        internal static void OnStructureSpawnedInvoker(StructureRegion region, StructureDrop drop)
        {
            OnStructureSpawned?.Invoke(region, drop);
        }

        internal static void OnPreStructureSpawnedInvoker(Structure structure, ItemStructureAsset asset,
            ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner,
            ref ulong @group, ref bool shouldAllow)
        {
            OnPreStructureSpawned?.Invoke(structure, asset, ref point, ref angle_x, ref angle_y, ref angle_z, ref owner,
                ref group, ref shouldAllow);
        }

        internal static void OnPreZombieDamagedInvoker(ref DamageZombieParameters parameters, ref bool shouldAllow)
        {
            OnPreZombieDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPreResourceDamagedInvoker(CSteamID instigatorSteamId, Transform transform,
            ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            OnPreResourceDamaged?.Invoke(instigatorSteamId, transform, ref pendingTotalDamage, ref shouldAllow,
                damageOrigin);
            if (!shouldAllow)
                return;
                
            Regions.tryGetCoordinate(transform.position, out var x, out var y);
            var resource = LevelGround.trees[x, y]
                .FirstOrDefault(resourceSpawnpoint => resourceSpawnpoint.model == transform);
            if (resource == null)
                return;
                
            if (resource.health <= pendingTotalDamage)
            {
                var player = PlayerTool.getPlayer(instigatorSteamId);
                if (player != null)
                {
                    if (!resource.asset.isForage && resource.asset.hasDebris)
                    {
                        var shouldAllow2 = true;
                        OnPrePlayerChoppedResource?.Invoke(player, resource, damageOrigin, ref shouldAllow2);
                        if (!shouldAllow2)
                            return;
                        
                        OnPlayerChoppedResource?.Invoke(player, resource, damageOrigin);
                    }

                    if (!resource.asset.isForage && !resource.asset.hasDebris)
                    {
                        var shouldAllow2 = true;
                        OnPrePlayerMinedResource?.Invoke(player, resource, damageOrigin, ref shouldAllow2);
                        if (!shouldAllow2)
                            return;
                        
                        OnPlayerMinedResource?.Invoke(player, resource, damageOrigin);
                    }
                }
            }

            OnResourceDamaged?.Invoke(instigatorSteamId, resource, pendingTotalDamage, damageOrigin);
        }

        internal static void OnPlayerPunchedInvoker(PlayerEquipment equipment, EPlayerPunch punch)
        {
            OnPlayerPunched?.Invoke(equipment.player, punch);
        }

        internal static void OnPlayerChangedEquipmentInvoker(PlayerEquipment equipment)
        {
            OnPlayerChangedEquipment?.Invoke(equipment.player);
        }

        internal static void OnPlayerInspectedEquipmentInvoker(PlayerEquipment equipment)
        {
            OnPlayerInspectedEquipment?.Invoke(equipment.player);
        }

        internal static void OnPlayerExperienceChangedInvoker(PlayerSkills skills, uint oldExperience)
        {
            OnPlayerExperienceChanged?.Invoke(skills.player, oldExperience, skills.experience);
        }

        internal static void OnPlayerReputationChangedInvoker(PlayerSkills skills, int oldReputation)
        {
            OnPlayerReputationChanged?.Invoke(skills.player, oldReputation, skills.reputation);
        }

        internal static void OnPlayerSkillUpgradedInvoker(PlayerSkills skills, byte speciality, byte index, byte level)
        {
            OnPlayerSkillUpgraded?.Invoke(skills.player, speciality, index, level);
        }

        internal static void OnPlayerChangedGestureInvoker(PlayerAnimator animator, EPlayerGesture gesture)
        {
            OnPlayerChangedGesture?.Invoke(animator.player, gesture);
        }

        internal static void OnPrePlayerCarjackedVehicleInvoker(InteractableVehicle vehicle, Player instigatingPlayer,
            ref bool allow, ref Vector3 force, ref Vector3 torque)
        {
            OnPrePlayerCarjackedVehicle?.Invoke(vehicle, instigatingPlayer, ref allow, ref force, ref torque);
        }

        internal static void OnPrePlayerLockpickedVehicleInvoker(InteractableVehicle vehicle, Player instigatingPlayer,
            ref bool allow)
        {
            OnPrePlayerLockpickedVehicle?.Invoke(vehicle, instigatingPlayer, ref allow);
        }

        internal static void OnPlayerBledInvoker(PlayerLife life)
        {
            OnPlayerBled?.Invoke(life.player);
        }

        internal static void OnPlayerBrokeBoneInvoker(PlayerLife life)
        {
            OnPlayerBrokeBone?.Invoke(life.player);
        }

        internal static void OnPlayerReloadedGunInvoker(UseableGun gun)
        {
            OnPlayerReloadedGun?.Invoke(gun);
        }

        internal static void OnPrePlayerGunHitEntityInvoker(UseableGun gun, BulletInfo bullet, InputInfo hit,
            ref bool shouldAllow)
        {
            OnPrePlayerGunHitEntity?.Invoke(gun, bullet, hit, ref shouldAllow);
        }

        internal static void OnPlayerThrewThrowableInvoker(UseableThrowable useable, GameObject throwable)
        {
            OnPlayerThrewThrowable?.Invoke(useable, throwable);
        }

        internal static void OnPlayerGunFiredInvoker(UseableGun gun, BulletInfo bullet)
        {
            OnPlayerGunFired?.Invoke(gun, bullet);
        }

        internal static void OnPlayerDiedInvoker(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            OnPlayerDied?.Invoke(sender, cause, limb, instigator);
        }

        internal static void OnVehicleDestroyedInvoker(InteractableVehicle vehicle)
        {
            OnVehicleDestroyed?.Invoke(vehicle);
        }

        internal static void OnVehicleExplodedInvoker(InteractableVehicle vehicle)
        {
            OnVehicleExploded?.Invoke(vehicle);
        }

        internal static void OnPrePlayerEnteredVehicleInvoker(Player player, InteractableVehicle vehicle, ref bool shouldallow)
        {
            OnPrePlayerEnteredVehicle?.Invoke(player, vehicle, ref shouldallow);
        }

        internal static void OnPrePlayerExitedVehicleInvoker(Player player, InteractableVehicle vehicle, ref bool shouldallow, ref Vector3 pendinglocation, ref float pendingyaw)
        {
            OnPrePlayerExitedVehicle?.Invoke(player, vehicle, ref shouldallow, ref pendinglocation, ref pendingyaw);
        }

        internal static void OnPreVehicleDamagedInvoker(CSteamID instigatorsteamid, InteractableVehicle vehicle, ref ushort pendingtotaldamage, ref bool canrepair, ref bool shouldallow, EDamageOrigin damageorigin)
        {
            OnPreVehicleDamaged?.Invoke(instigatorsteamid, vehicle, ref pendingtotaldamage, ref canrepair, ref shouldallow, damageorigin);
        }

        internal static void OnPreVehicleTireDamagedInvoker(CSteamID instigatorsteamid, InteractableVehicle vehicle, int tireindex, ref bool shouldallow, EDamageOrigin damageorigin)
        {
            OnPreVehicleTireDamaged?.Invoke(instigatorsteamid, vehicle, tireindex, ref shouldallow, damageorigin);
        }

        internal static void OnPlayerChangedStanceInvoker(PlayerStance obj)
        {
            OnPlayerChangedStance?.Invoke(obj);
        }

        internal static void OnPlayerChangedLeanInvoker(PlayerAnimator obj)
        {
            OnPlayerChangedLean?.Invoke(obj);
        }

        internal static void OnPlayerRevivedInvoker(PlayerLife obj)
        {
            OnPlayerRevived?.Invoke(obj);
        }

        internal static void OnVehicleFuelUpdatedInvoker(InteractableVehicle obj)
        {
            OnVehicleFuelUpdated?.Invoke(obj);
        }

        internal static void OnVehicleHealthUpdatedInvoker(InteractableVehicle obj)
        {
            OnVehicleHealthUpdated?.Invoke(obj);
        }

        internal static void OnVehicleLockUpdatedInvoker(InteractableVehicle obj)
        {
            OnVehicleLockUpdated?.Invoke(obj);
        }

        internal static void OnVehicleAddedPassengerInvoker(InteractableVehicle arg1, int arg2)
        {
            OnVehicleAddedPassenger?.Invoke(arg1, arg2);
        }

        internal static void OnVehicleRemovedPassengerInvoker(InteractableVehicle arg1, int arg2, Player arg3)
        {
            OnVehicleRemovedPassenger?.Invoke(arg1, arg2, arg3);
        }

        internal static void OnVehicleBatteryUpdatedInvoker(InteractableVehicle obj)
        {
            OnVehicleBatteryUpdated?.Invoke(obj);
        }

        internal static void OnVehiclePassengerChangedSeatInvoker(InteractableVehicle arg1, int arg2, int arg3)
        {
            OnVehiclePassengerChangedSeat?.Invoke(arg1, arg2, arg3);
        }

        internal static void OnPlayerHealthUpdatedInvoker(PlayerLife obj)
        {
            OnPlayerHealthUpdated?.Invoke(obj);
        }

        internal static void OnPlayerFoodUpdatedInvoker(PlayerLife obj)
        {
            OnPlayerFoodUpdated?.Invoke(obj);
        }

        internal static void OnPlayerWaterUpdatedInvoker(PlayerLife obj)
        {
            OnPlayerWaterUpdated?.Invoke(obj);
        }

        internal static void OnPlayerVirusUpdatedInvoker(PlayerLife obj)
        {
            OnPlayerVirusUpdated?.Invoke(obj);
        }

        internal static void OnPreVehicleRepairedInvoker(CSteamID instigatorsteamid, InteractableVehicle vehicle, ref ushort pendingtotalhealing, ref bool shouldallow)
        {
            OnPreVehicleRepaired?.Invoke(instigatorsteamid, vehicle, ref pendingtotalhealing, ref shouldallow);
        }

        internal static void OnPrePlayerSiphonedVehicleInvoker(InteractableVehicle vehicle, Player instigatingplayer, ref bool shouldallow, ref ushort desiredamount)
        {
            OnPrePlayerSiphonedVehicle?.Invoke(vehicle, instigatingplayer, ref shouldallow, ref desiredamount);
        }

        internal static void OnPrePlayerSwappedVehicleSeatInvoker(Player player, InteractableVehicle vehicle, ref bool shouldallow, byte fromseatindex, ref byte toseatindex)
        {
            OnPrePlayerSwappedVehicleSeat?.Invoke(player, vehicle, ref shouldallow, fromseatindex, ref toseatindex);
        }

        internal static void OnPreVehicleLockUpdatedInvoker(InteractableVehicle vehicle, ref bool shouldallow)
        {
            OnPreVehicleLockUpdated?.Invoke(vehicle, ref shouldallow);
        }

        internal static void OnPlayerChangedBackpackInvoker(PlayerClothing obj)
        {
            OnPlayerChangedBackpack?.Invoke(obj);
        }

        internal static void OnPlayerChangedGlassesInvoker(PlayerClothing obj)
        {
            OnPlayerChangedGlasses?.Invoke(obj);
        }

        internal static void OnPlayerChangedHatInvoker(PlayerClothing obj)
        {
            OnPlayerChangedHat?.Invoke(obj);
        }

        internal static void OnPlayerChangedMaskInvoker(PlayerClothing obj)
        {
            OnPlayerChangedMask?.Invoke(obj);
        }

        internal static void OnPlayerChangedPantsInvoker(PlayerClothing obj)
        {
            OnPlayerChangedPants?.Invoke(obj);
        }

        internal static void OnPlayerChangedShirtInvoker(PlayerClothing obj)
        {
            OnPlayerChangedShirt?.Invoke(obj);
        }

        internal static void OnPlayerChangedVestInvoker(PlayerClothing obj)
        {
            OnPlayerChangedVest?.Invoke(obj);
        }

        internal static void OnPlayerPluginKeyUpdatedInvoker(Player player, uint simulation, byte key, bool state)
        {
            OnPlayerPluginKeyUpdated?.Invoke(player, simulation, key, state);
        }

        public static void OnPrePlayerModifiedTireInvoker(UseableTire useable, InteractableVehicle vehicle, int wheelindex, ref bool shouldallow)
        {
            OnPrePlayerModifiedTire?.Invoke(useable, vehicle, wheelindex, ref shouldallow);
        }
    }
}
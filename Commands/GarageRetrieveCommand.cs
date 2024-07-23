using System;
using System.Threading.Tasks;
using RFGarage.DatabaseManagers;
using RFGarage.Enums;
using RFRocketLibrary.Models;
using Rocket.Unturned.Player;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using RocketExtensions.Utilities;
using UnityEngine;
using VehicleUtil = RFGarage.Utils.VehicleUtil;

namespace RFGarage.Commands
{
    [CommandActor(Rocket.API.AllowedCaller.Player)]
    [CommandPermissions("garageretrieve")]
#if RELEASEPUNCH
    [CommandAliases("vs")]
#else
    [CommandAliases("gretrieve", "gret", "gr", "garageget","gget")]
#endif
    [CommandInfo("Retrieve vehicle from garage.", "/garageretrieve <vehicleName>", AllowSimultaneousCalls = false)]
    public class GarageRetrieveCommand : RocketCommand
    {
        public override async Task Execute(CommandContext context)
        {
            if (context.CommandRawArguments.Length == 0)
            {
                await context.ReplyAsync(VehicleUtil.TranslateRich(EResponse.INVALID_PARAMETER.ToString(), Syntax),
                    RFGarage.Plugin.MsgColor, RFGarage.Plugin.Conf.MessageIconUrl);
                return;
            }

            var player = (UnturnedPlayer) context.Player;

            if (RFGarage.Plugin.Inst.IsProcessingGarage.TryGetValue(player.CSteamID.m_SteamID, out var lastProcessing) &&
                lastProcessing.HasValue && (DateTime.Now - lastProcessing.Value).TotalSeconds <= 1)
            {
                await context.ReplyAsync(VehicleUtil.TranslateRich(EResponse.PROCESSING_GARAGE.ToString()),
                    RFGarage.Plugin.MsgColor, RFGarage.Plugin.Conf.MessageIconUrl);
                return;
            }

            RFGarage.Plugin.Inst.IsProcessingGarage[player.CSteamID.m_SteamID] = null;
            Models.PlayerGarage playerGarage = null;
            var vehicleName = string.Join(" ", context.CommandRawArguments);
            if (byte.TryParse(vehicleName, out byte ind))
            {
                var fullGarage = await GarageManager.Get(player.CSteamID.m_SteamID);
                if(ind < fullGarage.Count) playerGarage = fullGarage[ind-1];
            }
            if(playerGarage == null)
            {
                playerGarage = await GarageManager.Get(player.CSteamID.m_SteamID, vehicleName);
                if (playerGarage == null)
                {
                    await context.ReplyAsync(VehicleUtil.TranslateRich(EResponse.VEHICLE_NOT_FOUND.ToString(), vehicleName),
                        RFGarage.Plugin.MsgColor, RFGarage.Plugin.Conf.MessageIconUrl);
                    return;
                }
            }
            RFGarage.Plugin.Inst.IsProcessingGarage[player.CSteamID.m_SteamID] = DateTime.Now;
            await DatabaseManager.Queue.Enqueue(async () => await GarageManager.DeleteAsync(playerGarage.Id))!;

            //Vehicle spawned position, based on player
            var pTransform = player.Player.transform;
            var point = pTransform.position + pTransform.forward * 6f;
            point += Vector3.up * 12f;
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                playerGarage.GarageContent.Position = point;
                playerGarage.GarageContent.Rotation = QuartenionWrapper.Create(pTransform.rotation);
                SDG.Unturned.InteractableVehicle veh = playerGarage.GarageContent.SpawnVehicle();
                if (veh.health < 1) veh.health = 1;
            });
            await context.ReplyAsync(
                VehicleUtil.TranslateRich(EResponse.GARAGE_RETRIEVE.ToString(),
                    playerGarage.GarageContent.GetVehicleAsset().vehicleName),
                RFGarage.Plugin.MsgColor, RFGarage.Plugin.Conf.MessageIconUrl);
        }
    }
}
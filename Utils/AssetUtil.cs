using System;
using System.Diagnostics;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFRocketLibrary.Utils
{
    public static class AssetUtil
    {
        #region Methods

        public static Asset GetAsset(ushort assetId, EAssetType assetType)
        {
            return Assets.find(assetType, assetId);
        }

        public static ItemAsset GetItemAsset(ushort itemId)
        {
            var asset = Assets.find(EAssetType.ITEM, itemId);
            return (ItemAsset) asset;
        }

        public static VehicleAsset GetVehicleAsset(ushort vehicleId)
        {
            var asset = Assets.find(EAssetType.VEHICLE, vehicleId);
            return (VehicleAsset) asset;
        }

        public static bool GiveItem(UnturnedPlayer player, Item item, ushort amount = 1,
            bool dropIfInventoryIsFull = true)
        {
            try
            {
                var added = false;

                for (var i = 0; i < amount; i++)
                {
                    added = player.Inventory.tryAddItem(item, true);
                    if (!added && dropIfInventoryIsFull)
                        ItemManager.dropItem(item, player.Position, true, true, true);
                }

                return added;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] AssetUtil GiveItem: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        public static bool GiveVehicle(UnturnedPlayer player, ushort id)
        {
            try
            {
                var pTransform = player.Player.transform;
                var point = pTransform.position + pTransform.forward * 6f;
                point += Vector3.up * 12f;
                return VehicleManager.spawnVehicleV2(id, point, pTransform.rotation) != null;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] AssetUtil GiveVehicle: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        #endregion
    }
}
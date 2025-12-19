using System;
using System.Diagnostics;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;

namespace RFRocketLibrary.Utils
{
    public static class InventoryUtil
    {
        #region Methods

        public static void ClearInventory(UnturnedPlayer player)
        {
            try
            {
                var playerInv = player.Inventory;

                // "Remove "models" of items from player "body""
                for (byte index = 0; index < player.Inventory.getItemCount(0); index++)
                {
                    var item = player.Inventory.getItem(0, index);
                    if (item != null)
                        player.Inventory.removeItem(0, index);
                }

                player.Player.equipment.sendSlot(0);

                for (byte index = 0; index < player.Inventory.getItemCount(1); index++)
                {
                    var item = player.Inventory.getItem(1, index);
                    if (item != null)
                        player.Inventory.removeItem(1, index);
                }

                player.Player.equipment.sendSlot(1);

                // Remove items
                for (byte page = 0; page < 7; page++)
                {
                    var count = playerInv.getItemCount(page);

                    for (byte index = 0; index < count; index++)
                        playerInv.removeItem(page, 0);
                }

                // Remove clothes

                // Remove unequipped cloths
                void RemoveUnequipped()
                {
                    for (byte i = 0; i < playerInv.getItemCount(2); i++)
                        playerInv.removeItem(2, 0);
                }

                // Unequip & remove from inventory
                player.Inventory.items[2].resize(10, 50);
                player.Player.clothing.askWearBackpack(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearGlasses(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearHat(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearPants(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearMask(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearShirt(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();

                player.Player.clothing.askWearVest(0, 0, Array.Empty<byte>(), true);
                RemoveUnequipped();
                player.Inventory.items[2].resize(5, 3);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] InventoryUtil ClearInventory: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
            }
        }

        #endregion
    }
}
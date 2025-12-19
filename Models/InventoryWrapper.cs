using System;
using System.Collections.Generic;
using RFRocketLibrary.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class InventoryWrapper
    {
        public List<ItemWrapper> Clothing { get; set; } = new();
        public List<ItemsWrapper> Items { get; set; } = new();

        public InventoryWrapper()
        {
        }

        public InventoryWrapper(List<ItemWrapper> clothing, List<ItemsWrapper> items)
        {
            Clothing = clothing;
            Items = items;
        }

        public static InventoryWrapper Create(PlayerClothing playerClothing, PlayerInventory playerInventory)
        {
            var clothing = new List<ItemWrapper>();
            if (playerClothing.backpack != 0)
                clothing.Add(new ItemWrapper(playerClothing.backpack, 1, playerClothing.backpackQuality,
                    playerClothing.backpackState));

            if (playerClothing.glasses != 0)
                clothing.Add(new ItemWrapper(playerClothing.glasses, 1, playerClothing.glassesQuality,
                    playerClothing.glassesState));

            if (playerClothing.hat != 0)
                clothing.Add(new ItemWrapper(playerClothing.hat, 1, playerClothing.hatQuality,
                    playerClothing.hatState));

            if (playerClothing.mask != 0)
                clothing.Add(new ItemWrapper(playerClothing.mask, 1, playerClothing.maskQuality,
                    playerClothing.maskState));

            if (playerClothing.pants != 0)
                clothing.Add(new ItemWrapper(playerClothing.pants, 1, playerClothing.pantsQuality,
                    playerClothing.pantsState));

            if (playerClothing.shirt != 0)
                clothing.Add(new ItemWrapper(playerClothing.shirt, 1, playerClothing.shirtQuality,
                    playerClothing.shirtState));

            if (playerClothing.vest != 0)
                clothing.Add(new ItemWrapper(playerClothing.vest, 1, playerClothing.vestQuality,
                    playerClothing.vestState));

            var items = new List<ItemsWrapper>();
            foreach (var inventoryItem in playerInventory.items)
                if (inventoryItem?.items != null && inventoryItem.items.Count != 0)
                    items.Add(ItemsWrapper.Create(inventoryItem));

            return new InventoryWrapper(clothing, items);
        }

        public void GiveInventory(UnturnedPlayer player, bool replaceInventory = false)
        {
            switch (replaceInventory)
            {
                case true:
                    InventoryUtil.ClearInventory(player);
                    foreach (var clothing in Clothing)
                        clothing.GiveItem(player);

                    foreach (var itemsWrapper in Items)
                        itemsWrapper.GiveTo(player);
                    break;
                case false:
                    foreach (var clothing in Clothing)
                        clothing.GiveItem(player);

                    foreach (var itemsWrapper in Items)
                        itemsWrapper.GiveTo(player);
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class ItemsWrapper
    {
        public byte Page { get; set; }
        public byte Height { get; set; }
        public byte Width { get; set; }
        public List<ItemJarWrapper> Items { get; set; } = new();

        public ItemsWrapper()
        {
            
        }

        public ItemsWrapper(byte page, byte height, byte width, List<ItemJarWrapper> items)
        {
            Page = page;
            Height = height;
            Width = width;
            Items = items;
        }

        public static ItemsWrapper Create(Items items)
        {
            return new ItemsWrapper(items.page, items.height, items.width, items.items.Select(ItemJarWrapper.Create).ToList());
        }

        public Items ToItems()
        {
            var items = new Items(Page);
            items.resize(Width, Height);
            foreach (var itemJarWrapper in Items)
                items.addItem(itemJarWrapper.X, itemJarWrapper.Y, itemJarWrapper.Rotation, itemJarWrapper.Item.ToItem());

            return items;
        }
        
        public void GiveTo(UnturnedPlayer player, bool replacePage = false)
        {
            switch (replacePage)
            {
                case true:
                    player.Inventory.updateItems(Page, ToItems());
                    break;
                case false:
                    foreach (var itemJarWrapper in Items)
                        itemJarWrapper.GiveTo(player);
                    break;
            }
        }
    }
}
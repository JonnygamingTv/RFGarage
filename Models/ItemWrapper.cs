using System;
using RFRocketLibrary.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class ItemWrapper
    {
        public ushort Id { get; set; }
        public byte Amount { get; set; }
        public byte Quality { get; set; }
        public byte[] State { get; set; } = Array.Empty<byte>();

        public ItemWrapper()
        {
        }

        public ItemWrapper(ushort id, byte amount, byte quality, byte[] state)
        {
            Id = id;
            Amount = amount;
            Quality = quality;
            State = state;
        }

        public static ItemWrapper Create(Item item)
        {
            return new ItemWrapper(item.id, item.amount, item.quality, item.state);
        }

        public virtual Item ToItem() => new(Id, Amount, Quality, State);

        public virtual bool GiveItem(UnturnedPlayer player, bool dropIfInventoryFull = true)
        {
            return AssetUtil.GiveItem(player, ToItem(), 1, dropIfInventoryFull);
        }

        public ItemAsset GetItemAsset()
        {
            return (ItemAsset) Assets.find(EAssetType.ITEM, Id);
        }

        public override string ToString()
        {
            var itemName = (Assets.find(EAssetType.ITEM, Id) as ItemAsset)?.itemName ?? "Unknown Name";
            return $"ItemID: {Id} ({itemName}), Durability: {Quality}, Amount: {Amount}";
        }
    }
}
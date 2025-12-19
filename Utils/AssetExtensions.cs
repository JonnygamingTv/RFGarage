using RFRocketLibrary.Helpers;
using SDG.Unturned;

namespace RFRocketLibrary.Utils
{
    public static class AssetExtensions
    {
        #region Methods

        public static string GetImage(this VehicleAsset asset) => ImageHelper.GetVehicleImage(asset);
        public static string GetImage(this ItemAsset asset) => ImageHelper.GetItemImage(asset);

        #endregion
    }
}
using SDG.Unturned;

namespace RFRocketLibrary.Helpers
{
    public static class ImageHelper
    {
        #region Methods

        public static string GetItemImage(ItemAsset asset, bool mirror = false) => asset.assetOrigin == EAssetOrigin.OFFICIAL
            ? GetVanillaItemImage(asset.id.ToString(), mirror)
            : GetWorkshopItemImage(asset.originMasterBundle?.assetBundleNameWithoutExtension ?? "unknown",
                asset.id.ToString(), mirror);

        public static string GetPluginImage(string pluginName, string imageName, bool mirror = false) =>
            string.Format(mirror ? PluginMirror : Plugin, pluginName, imageName);

        public static string GetVanillaItemImage(string id, bool mirror = false) => string.Format(mirror ? VanillaItemMirror : VanillaItem, id);

        public static string GetVanillaVehicleImage(string id, bool mirror = false) => string.Format(mirror ? VanillaVehicleMirror : VanillaVehicle, id);
        public static string GetEconItemImage(string id, bool mirror = false) => string.Format(mirror ? EconItemMirror : EconItem, id);
        public static string GetEconVehicleImage(string id, bool mirror = false) => string.Format(mirror ? EconVehicleMirror : EconVehicle, id);

        public static string GetVehicleImage(VehicleAsset asset, bool mirror = false) => asset.assetOrigin == EAssetOrigin.OFFICIAL
            ? GetVanillaVehicleImage(asset.id.ToString(), mirror)
            : GetWorkshopVehicleImage(asset.originMasterBundle?.assetBundleNameWithoutExtension ?? "unknown",
                asset.id.ToString(), mirror);

        public static string GetWorkshopItemImage(string assetBundleName, string id, bool mirror = false) =>
            string.Format(mirror ? WorkshopItemMirror : WorkshopItem, assetBundleName, id);

        public static string GetWorkshopVehicleImage(string assetBundleName, string id, bool mirror = false) =>
            string.Format(mirror ? WorkshopVehicleMirror : WorkshopVehicle, assetBundleName, id);

        #endregion

        public static string VanillaItem =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/vanilla/items/{0}.png";

        public static string VanillaItemMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/vanilla/items/{0}.png";

        public static string EconItem =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/econ/items/{0}.png";

        public static string EconItemMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/vanilla/items/{0}.png";

        public static string WorkshopItem =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/workshop/items/{0}/{1}.png";

        public static string WorkshopItemMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/workshop/items/{0}/{1}.png";

        public static string VanillaVehicle =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/vanilla/vehicles/{0}.png";

        public static string VanillaVehicleMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/vanilla/vehicles/{0}.png";

        public static string EconVehicle =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/econ/vehicles/{0}.png";

        public static string EconVehicleMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/econ/vehicles/{0}.png";

        public static string WorkshopVehicle =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/workshop/vehicles/{0}/{1}.png";

        public static string WorkshopVehicleMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/workshop/vehicles/{0}/{1}.png";

        public static string Plugin =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/{0}/{1}.png";

        public static string PluginMirror =>
            "https://gitlab.com/RiceField-Plugins/UnturnedImages/-/raw/images/plugin/{0}/{1}.png";
    }
}
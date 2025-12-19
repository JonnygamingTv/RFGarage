using SDG.NetTransport;
using SDG.Unturned;

namespace RFRocketLibrary.Utils
{
    public static class UIUtil
    {
        #region Methods

        public static void Initialize(ushort id, short key, ITransportConnection transportConnection, bool fast = true)
        {
            EffectManager.sendUIEffect(id, key, transportConnection, !fast);
        }

        public static void SetImage(short key, ITransportConnection transportConnection, string effectName, string value = "", bool fast = true)
        {
            EffectManager.sendUIEffectImageURL(key, transportConnection, !fast, effectName, value);
        }

        public static void SetText(short key, ITransportConnection transportConnection, string effectName, string value = "", bool fast = true)
        {
            EffectManager.sendUIEffectText(key, transportConnection, !fast, effectName, value);
        }

        public static void SetVisibility(short key, ITransportConnection transportConnection, string effectName, bool value, bool fast = true)
        {
            EffectManager.sendUIEffectVisibility(key, transportConnection, !fast, effectName, value);
        }

        #endregion
    }
}
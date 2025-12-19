using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RFRocketLibrary.Utils;
using Rocket.Core;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Hooks
{
    // ReSharper disable once InconsistentNaming
    public static class RFPlayerLibraryHook
    {
        #region Methods

        public static bool CanBeLoaded()
        {
            return R.Plugins.GetPlugins().Any(c => c.Name.ToLower() == "rfplayerlibrary") && !_initialized;
        }

        public static IDictionary<string, object>? GetPlayerGeolocationInfoBySteamId(ulong steamId)
        {
            return _getGeolocationInfoBySteamIdMethod?.Invoke(_playerGeolocationManagerInstance, new object[]
            {
                steamId
            }) as IDictionary<string, object>;
        }

        public static List<IDictionary<string, object>>? GetPlayerInfoByName(string name)
        {
            return _getInfoByNameMethod?.Invoke(_playerManagerInstance, new object[]
            {
                name
            }) as List<IDictionary<string, object>>;
        }

        public static IDictionary<string, object>? GetPlayerInfoBySteamId(ulong steamId)
        {
            return _getInfoBySteamIdMethod?.Invoke(_playerManagerInstance, new object[]
            {
                steamId
            }) as IDictionary<string, object>;
        }

        public static void Load()
        {
            if (_initialized)
                return;
            try
            {
                Logger.LogWarning("[RFPlayerLibraryHook] Loading RFPlayerLibrary hook...");

                var playerLibraryPlugin =
                    R.Plugins.GetPlugins().FirstOrDefault(c => c.Name.ToLower() == "rfplayerlibrary");
                var playerLibraryType = playerLibraryPlugin?.GetType().Assembly.GetType("RFPlayerLibrary.Plugin");
                var playerLibraryInstance =
                    playerLibraryType?.GetField("Inst", BindingFlags.Static | BindingFlags.Public)
                        ?.GetValue(playerLibraryPlugin);
                _databaseInstance = playerLibraryInstance?.GetType().GetField("Database")?
                    .GetValue(playerLibraryInstance);

                #region Method

                Logger.LogWarning("[RFPlayerLibraryHook] Obtaining methods...");
                _playerManagerInstance =
                    _databaseInstance?.GetType().GetField("PlayerManager")?.GetValue(_databaseInstance);
                _getInfoBySteamIdMethod = ReflectionHelper.GetMethod(_playerManagerInstance?.GetType(),
                    "GetInfoBySteamId", new[] {typeof(ulong)});
                _getInfoByNameMethod = ReflectionHelper.GetMethod(_playerManagerInstance?.GetType(),
                    "GetInfoByName", new[] {typeof(string)});
                _playerGeolocationManagerInstance = _databaseInstance?.GetType().GetField("PlayerGeolocationManager")?
                    .GetValue(_databaseInstance);
                _getGeolocationInfoBySteamIdMethod = ReflectionHelper.GetMethod(_playerGeolocationManagerInstance?.GetType(),
                    "GetInfoBySteamId", new[] {typeof(ulong)});
                Logger.LogWarning("[RFPlayerLibraryHook] Methods obtained.");

                #endregion

                Logger.LogWarning("[RFPlayerLibraryHook] RFPlayerLibrary hook loaded.");
                _initialized = true;
            }
            catch (Exception e)
            {
                Logger.LogError($"[RFPlayerLibraryHook] Failed to hook RFPlayerLibrary: {e.Message}");
                Logger.LogError($"[RFPlayerLibraryHook] Details: {e}");
            }
        }

        #endregion

        private static bool _initialized;
        private static object? _databaseInstance;
        private static object? _playerManagerInstance;
        private static object? _playerGeolocationManagerInstance;
        private static MethodInfo? _getInfoBySteamIdMethod;
        private static MethodInfo? _getInfoByNameMethod;
        private static MethodInfo? _getGeolocationInfoBySteamIdMethod;
    }
}
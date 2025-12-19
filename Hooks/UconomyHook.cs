using System;
using System.Linq;
using System.Reflection;
using RFRocketLibrary.Utils;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;

namespace RFRocketLibrary.Hooks
{
    public static class UconomyHook
    {
        private static bool _initialized;
        private static MethodInfo? _getBalanceMethod;
        private static MethodInfo? _increaseBalanceMethod;
        private static EventInfo? _onBalanceUpdateEvent;
        private static object? _uconomyInstance;
        private static object? _databaseInstance;
#if LP
        private static MethodInfo _addHistoryMethod;
        private static object _databaseHistoryInstance;
#endif

        public static decimal InitialBalance { get; private set; }
        public static string? MoneySymbol { get; private set; }
        public static string? MoneyName { get; private set; }
        public static string? MessageColor { get; private set; }

        public delegate void BalanceUpdated(UnturnedPlayer player, decimal amt);

        public static event BalanceUpdated? OnBalanceUpdated;

        public static void Load()
        {
            if (_initialized)
                return;
            try
            {
                Logger.LogWarning("[UconomyHook] Loading Uconomy hook...");

                var uconomyPlugin = R.Plugins.GetPlugins().FirstOrDefault(c => c.Name.ToLower().Contains("uconomy"));
                if (uconomyPlugin == null)
                    throw new NullReferenceException("Uconomy plugin not found!");
                var uconomyType = uconomyPlugin.GetType().Assembly.GetType("fr34kyn01535.Uconomy.Uconomy");
                _uconomyInstance =
                    uconomyType?.GetField("Instance", BindingFlags.Static | BindingFlags.Public)
                        ?.GetValue(uconomyPlugin);
                _databaseInstance = _uconomyInstance?.GetType().GetField("Database")?.GetValue(_uconomyInstance);
#if LP
                _databaseHistoryInstance =
 _uconomyInstance?.GetType().GetField("DatabaseHistory")?.GetValue(_uconomyInstance);
#endif

                #region Method

                Logger.LogWarning("[UconomyHook] Obtaining methods...");
                _getBalanceMethod = ReflectionHelper.GetMethod(_databaseInstance?.GetType(),
                    "GetBalance", new[] {typeof(string)});
                _increaseBalanceMethod = ReflectionHelper.GetMethod(_databaseInstance?.GetType(),
                    "IncreaseBalance", new[] {typeof(string), typeof(decimal)});
#if LP
                _addHistoryMethod = ReflectUtil.GetMethod(_databaseHistoryInstance?.GetType(),
                    "Add", new[] {typeof(ulong), typeof(decimal), typeof(string)});
#endif
                Logger.LogWarning("[UconomyHook] Methods obtained.");

                #endregion

                #region Configuration

                Logger.LogWarning("[UconomyHook] Obtaining configuration...");
                var configuration = _uconomyInstance?.GetType().GetProperty("Configuration")
                    ?.GetValue(_uconomyInstance);
                var configurationType = configuration?.GetType();
                var configurationInstance = configurationType
                    ?.GetProperty("Instance", BindingFlags.Instance | BindingFlags.Public)
                    ?.GetValue(configuration);
                var configurationInstanceType = configurationInstance?.GetType();
                InitialBalance = Convert.ToDecimal(configurationInstanceType?.GetField("InitialBalance")
                    ?.GetValue(configurationInstance));
                MoneySymbol = Convert.ToString(configurationInstanceType?.GetField("MoneySymbol")
                    ?.GetValue(configurationInstance));
                MoneyName = Convert.ToString(configurationInstanceType?.GetField("MoneyName")
                    ?.GetValue(configurationInstance));
                MessageColor = Convert.ToString(configurationInstanceType?.GetField("MessageColor")
                    ?.GetValue(configurationInstance));
                Logger.LogWarning("[UconomyHook] Configuration obtained.");

                #endregion

                #region Events

                Logger.LogWarning("[UconomyHook] Obtaining events...");
                _onBalanceUpdateEvent = _uconomyInstance?.GetType()
                    .GetEvent("OnBalanceUpdate", BindingFlags.Instance | BindingFlags.Public);
                var reflectOnBalanceUpdateMethod =
                    typeof(UconomyHook).GetMethod("ReflectOnBalanceUpdate",
                        BindingFlags.Static | BindingFlags.Public);
                if (_onBalanceUpdateEvent == null || reflectOnBalanceUpdateMethod == null)
                    Logger.LogError("[UconomyHook] OnBalanceUpdate event couldn't be loaded...");
                if (_onBalanceUpdateEvent?.EventHandlerType != null)
                {
                    if (reflectOnBalanceUpdateMethod is not null)
                    {
                        var balanceUpdate =
                            Delegate.CreateDelegate(_onBalanceUpdateEvent.EventHandlerType, null,
                                reflectOnBalanceUpdateMethod);
                        _onBalanceUpdateEvent.GetAddMethod().Invoke(_uconomyInstance, new object[] {balanceUpdate});
                    }
                }

                Logger.LogWarning("[UconomyHook] Events obtained.");

                #endregion

                Logger.LogWarning("[UconomyHook] Uconomy hook loaded.");
                _initialized = true;
            }
            catch (Exception e)
            {
                Logger.LogError($"[UconomyHook] Failed to hook Uconomy: {e.Message}");
                Logger.LogError($"[UconomyHook] Details: {e}");
            }
        }

        public static bool CanBeLoaded()
        {
            return R.Plugins.GetPlugins().Any(c => c.Name.ToLower() == "uconomy") && !_initialized;
        }

        public static decimal Withdraw(ulong steamId, decimal amount)
        {
            return _increaseBalanceMethod != null
                ? (decimal) _increaseBalanceMethod.Invoke(_databaseInstance, new object[]
                {
                    steamId.ToString(), -amount
                })
                : default;
        }

        public static decimal Deposit(ulong steamId, decimal amount)
        {
            return _increaseBalanceMethod != null
                ? (decimal) _increaseBalanceMethod.Invoke(_databaseInstance, new object[]
                {
                    steamId.ToString(), amount
                })
                : default;
        }

        public static decimal GetBalance(ulong steamId)
        {
            return _getBalanceMethod != null
                ? (decimal) _getBalanceMethod.Invoke(_databaseInstance, new object[] {steamId.ToString()})
                : default;
        }

        public static bool Has(ulong steamId, decimal amount)
        {
            return GetBalance(steamId) >= amount;
        }

#if LP
        public static void AddHistory(ulong steamId, decimal balanceDelta, string description = "")
        {
            _addHistoryMethod?.Invoke(_databaseHistoryInstance, new object[] {steamId, balanceDelta, description});
        }
#endif

        public static void ReflectOnBalanceUpdate(UnturnedPlayer player, decimal amt)
        {
            OnBalanceUpdated?.Invoke(player, amt);
        }
    }
}
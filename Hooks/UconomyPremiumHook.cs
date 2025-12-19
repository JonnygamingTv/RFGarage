using System;
using System.Linq;
using System.Reflection;
using RFRocketLibrary.Utils;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;

namespace RFRocketLibrary.Hooks
{
    public static class UconomyPremiumHook
    {
        #region Delegates

        public delegate void BalanceUpdated(UnturnedPlayer player, decimal amt);

        #endregion

        #region Events

        public static event BalanceUpdated? OnBalanceUpdated;

        #endregion

        #region Methods

        public static void AddHistory(ulong steamId, decimal balanceDelta, string description = "")
        {
            _addHistoryMethod?.Invoke(_databaseHistoryInstance, new object[] {steamId, balanceDelta, description});
        }

        public static bool CanBeLoaded()
        {
            return R.Plugins.GetPlugins().Any(c => c.Name.ToLower() == "uconomy") && !_initialized;
        }

        public static decimal? Deposit(ulong steamId, decimal amount)
        {
            return _increaseBalanceMethod?.Invoke(_databaseInstance, new object[]
                {
                    steamId.ToString(), amount
                }) as decimal?;
        }

        public static decimal? GetBalance(ulong steamId)
        {
            return _getBalanceMethod?.Invoke(_databaseInstance, new object[] {steamId.ToString()}) as decimal?;
        }

        public static bool Has(ulong steamId, decimal amount)
        {
            return GetBalance(steamId) >= amount;
        }

        public static void Load()
        {
            if (_initialized)
                return;
            try
            {
                Logger.LogWarning("[UconomyPremiumHook] Loading UconomyPremium hook...");

                var uconomyPlugin = R.Plugins.GetPlugins().FirstOrDefault(c => c.Name.ToLower() == "uconomy");
                var uconomyType = uconomyPlugin?.GetType().Assembly.GetType("fr34kyn01535.Uconomy.Uconomy");
                _uconomyInstance =
                    uconomyType?.GetField("Instance", BindingFlags.Static | BindingFlags.Public)
                        ?.GetValue(uconomyPlugin);
                _databaseInstance = _uconomyInstance?.GetType().GetField("DatabasePremium")?.GetValue(_uconomyInstance);
                _databaseHistoryInstance = _uconomyInstance?.GetType().GetField("DatabasePremiumHistory")
                    ?.GetValue(_uconomyInstance);

                #region Method

                Logger.LogWarning("[UconomyPremiumHook] Obtaining methods...");
                _getBalanceMethod = ReflectionHelper.GetMethod(_databaseInstance?.GetType(),
                    "GetBalance", new[] {typeof(string)});
                _increaseBalanceMethod = ReflectionHelper.GetMethod(_databaseInstance?.GetType(),
                    "IncreaseBalance", new[] {typeof(string), typeof(decimal)});
                _addHistoryMethod = ReflectionHelper.GetMethod(_databaseHistoryInstance?.GetType(),
                    "Add", new[] {typeof(ulong), typeof(decimal), typeof(string)});
                Logger.LogWarning("[UconomyPremiumHook] Methods obtained.");

                #endregion

                #region Configuration

                Logger.LogWarning("[UconomyPremiumHook] Obtaining configuration...");
                var configuration = _uconomyInstance?.GetType().GetProperty("Configuration")
                    ?.GetValue(_uconomyInstance);
                var configurationType = configuration?.GetType();
                var configurationInstance = configurationType
                    ?.GetProperty("Instance", BindingFlags.Instance | BindingFlags.Public)
                    ?.GetValue(configuration);
                var configurationInstanceType = configurationInstance?.GetType();
                InitialBalance = Convert.ToDecimal(configurationInstanceType?.GetField("InitialPremiumBalance")
                    ?.GetValue(configurationInstance));
                MoneySymbol = Convert.ToString(configurationInstanceType?.GetField("PremiumMoneySymbol")
                    ?.GetValue(configurationInstance));
                MoneyName = Convert.ToString(configurationInstanceType?.GetField("PremiumMoneyName")
                    ?.GetValue(configurationInstance));
                MessageColor = Convert.ToString(configurationInstanceType?.GetField("PremiumMessageColor")
                    ?.GetValue(configurationInstance));
                Logger.LogWarning("[UconomyPremiumHook] Configuration obtained.");

                #endregion

                #region Events

                Logger.LogWarning("[UconomyPremiumHook] Obtaining events...");
                _onBalanceUpdateEvent = _uconomyInstance?.GetType()
                    .GetEvent("OnBalancePremiumUpdate", BindingFlags.Instance | BindingFlags.Public);
                var reflectOnBalanceUpdateMethod =
                    typeof(UconomyPremiumHook).GetMethod("ReflectOnBalanceUpdate",
                        BindingFlags.Static | BindingFlags.Public);
                if (_onBalanceUpdateEvent == null || reflectOnBalanceUpdateMethod == null)
                    Logger.LogError("[UconomyPremiumHook] OnBalanceUpdate event couldn't be loaded...");
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

                Logger.LogWarning("[UconomyPremiumHook] Events obtained.");

                #endregion

                Logger.LogWarning("[UconomyPremiumHook] UconomyPremium hook loaded.");
                _initialized = true;
            }
            catch (Exception e)
            {
                Logger.LogError($"[UconomyPremiumHook] Failed to hook UconomyPremium: {e.Message}");
                Logger.LogError($"[UconomyPremiumHook] Details: {e}");
            }
        }

        public static void ReflectOnBalanceUpdate(UnturnedPlayer player, decimal amt)
        {
            OnBalanceUpdated?.Invoke(player, amt);
        }

        public static decimal? Withdraw(ulong steamId, decimal amount)
        {
            return _increaseBalanceMethod?.Invoke(_databaseInstance, new object[]
            {
                steamId.ToString(), -amount
            }) as decimal?;
        }

        #endregion

        private static bool _initialized;
        private static MethodInfo? _getBalanceMethod;
        private static MethodInfo? _increaseBalanceMethod;
        private static MethodInfo? _addHistoryMethod;
        private static EventInfo? _onBalanceUpdateEvent;
        private static object? _uconomyInstance;
        private static object? _databaseInstance;
        private static object? _databaseHistoryInstance;

        public static decimal InitialBalance { get; private set; }
        public static string? MoneySymbol { get; private set; }
        public static string? MoneyName { get; private set; }
        public static string? MessageColor { get; private set; }
    }
}
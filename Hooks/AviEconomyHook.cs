using System;
using System.Linq;
using System.Reflection;
using RFRocketLibrary.Utils;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;

namespace RFRocketLibrary.Hooks
{
    public static class AviEconomyHook
    {
        #region Delegates

        public delegate void BalanceChanged(string playerId, decimal oldAmount, decimal toValue);

        #endregion

        #region Events

        public static event BalanceChanged? OnBalanceChanged;

        #endregion

        #region Methods

        public static bool CanBeLoaded() =>
            R.Plugins.GetPlugins().Any(c => c.Name.ToLower() == "avieconomy") && !_initialized;

        public static decimal? Deposit(UnturnedPlayer? player, decimal amount)
        {
            // args[3] = out decimal pFinalBalance
            var args = new object?[] {player, amount, false, null, null};
            _payAsServerMethod?.Invoke(_bankType, args);
            return args[3] as decimal?;
        }

        public static decimal GetBalance(UnturnedPlayer player) =>
            _getBalanceMethod != null ? (decimal) _getBalanceMethod.Invoke(_bankType, new object[] {player.Id}) : default;

        public static bool Has(UnturnedPlayer player, decimal amount) => GetBalance(player) - amount >= 0;

        public static void Load()
        {
            if (_initialized)
                return;
            try
            {
                Logger.LogWarning("[AviEconomyHook] Loading AviEconomy hook...");

                var economyPlugin = R.Plugins.GetPlugins().FirstOrDefault(c => c.Name.ToLower() == "avieconomy");
                if (economyPlugin == null)
                    throw new Exception("[AviEconomyHook] AviEconomy Plugin couldn't be loaded...");
                var economyType = economyPlugin.GetType().Assembly
                    .GetType("avirockets.unturned.AviEconomy.EconomyPlugin");

                #region Configuration

                Logger.LogWarning("[AviEconomyHook] Obtaining configuration...");
                var configuration = economyType?.GetProperty("CfgAsset")?.GetValue(economyPlugin);
                var configurationInstance = configuration?.GetType()
                    .GetProperty("Instance", BindingFlags.Instance | BindingFlags.Public)?.GetValue(configuration);
                var configurationInstanceType = configurationInstance?.GetType();
                InitialBalance = Convert.ToDecimal(configurationInstanceType?.GetField("InitialBalance")?
                    .GetValue(configurationInstance));
                XpMode = Convert.ToBoolean(
                    configurationInstanceType?.GetField("XpMode").GetValue(configurationInstance));
                XpModeUseInitialBalance = Convert.ToBoolean(configurationInstanceType
                    ?.GetField("XpModeUseInitialBalance")?
                    .GetValue(configurationInstance));
                CurrencyName =
                    Convert.ToString(configurationInstanceType?.GetField("CurrencyName")
                        ?.GetValue(configurationInstance));
                CurrencySymbol = Convert.ToString(configurationInstanceType?.GetField("CurrencySymbol")?
                    .GetValue(configurationInstance));
                CurrencySymbolBeforeValue = Convert.ToBoolean(configurationInstanceType
                    ?.GetField("CurrencySymbolBeforeValue").GetValue(configurationInstance));
                SalaryInterval = Convert.ToSingle(configurationInstanceType?.GetField("SalaryInterval")?
                    .GetValue(configurationInstance));
                ExperienceExchangeRate = Convert.ToSingle(configurationInstanceType?.GetField("ExperienceExchangeRate")?
                    .GetValue(configurationInstance));
                MoneyExchangeRate = Convert.ToSingle(configurationInstanceType?.GetField("MoneyExchangeRate")?
                    .GetValue(configurationInstance));
                RewardForZombieHead = Convert.ToDecimal(configurationInstanceType?.GetField("RewardForZombieHead")?
                    .GetValue(configurationInstance));
                RewardForMegaZombieHead = Convert.ToDecimal(configurationInstanceType
                    ?.GetField("RewardForMegaZombieHead")
                    .GetValue(configurationInstance));
                MaxPaidZombiesPerHour = Convert.ToDecimal(configurationInstanceType?.GetField("MaxPaidZombiesPerHour")?
                    .GetValue(configurationInstance));
                MaxPaidZombiesPerDay = Convert.ToDecimal(configurationInstanceType?.GetField("MaxPaidZombiesPerDay")?
                    .GetValue(configurationInstance));
                ShowZombieRewardMessages = Convert.ToBoolean(configurationInstanceType
                    ?.GetField("ShowZombieRewardMessages")?
                    .GetValue(configurationInstance));
                ShowZombieOverLimitMessages = Convert.ToBoolean(configurationInstanceType
                    ?.GetField("ShowZombieOverLimitMessages")?.GetValue(configurationInstance));
                RewardForPlayerHead = Convert.ToDecimal(configurationInstanceType?.GetField("RewardForPlayerHead")?
                    .GetValue(configurationInstance));
                PenaltyForDeath = Convert.ToDecimal(configurationInstanceType?.GetField("PenaltyForDeath")?
                    .GetValue(configurationInstance));
                PenaltyForSuicide = Convert.ToString(configurationInstanceType?.GetField("PenaltyForSuicide")?
                    .GetValue(configurationInstance));
                BalanceBgEffectId = Convert.ToUInt16(configurationInstanceType?.GetField("BalanceBgEffectId")?
                    .GetValue(configurationInstance));
                BalanceFgEffectId = Convert.ToUInt16(configurationInstanceType?.GetField("BalanceFgEffectId")?
                    .GetValue(configurationInstance));
                BalanceTextColor = Convert.ToString(configurationInstanceType?.GetField("BalanceTextColor")?
                    .GetValue(configurationInstance));
                AddCurrencySymbolToUI = Convert.ToBoolean(configurationInstanceType?.GetField("AddCurrencySymbolToUI")?
                    .GetValue(configurationInstance));
                UseOpenModEconomy = Convert.ToBoolean(configurationInstanceType?.GetField("UseOpenModEconomy")?
                    .GetValue(configurationInstance));
                MySqlConnectionString = Convert.ToString(configurationInstanceType?.GetField("MySqlConnectionString")?
                    .GetValue(configurationInstance));
                MySqlTableNamePrefix = Convert.ToString(configurationInstanceType?.GetField("MySqlTableNamePrefix")?
                    .GetValue(configurationInstance));
                Logger.LogWarning("[AviEconomyHook] Configuration obtained.");

                #endregion

                #region Method

                Logger.LogWarning("[AviEconomyHook] Obtaining methods...");
                _bankType = economyPlugin.GetType().Assembly.GetType("avirockets.unturned.AviEconomy.Bank");
                if (_bankType == null)
                    Logger.LogError("[AviEconomyHook] AviEconomy Bank type couldn't be loaded...");
                _getBalanceMethod = ReflectionHelper.GetMethod(_bankType, "GetBalance",
                    BindingFlags.Static | BindingFlags.Public,
                    new[] {typeof(string)});
                if (_getBalanceMethod == null)
                    Logger.LogError("[AviEconomyHook] AviEconomy GetBalance method couldn't be loaded...");
                _payAsServerMethod = ReflectionHelper.GetMethod(_bankType, "PayAsServer",
                    BindingFlags.Static | BindingFlags.Public,
                    new[]
                    {
                        typeof(IRocketPlayer), typeof(decimal), typeof(bool), typeof(decimal).MakeByRefType(),
                        typeof(string)
                    });
                if (_payAsServerMethod == null)
                    Logger.LogError("[AviEconomyHook] AviEconomy PayAsServer method couldn't be loaded...");
                Logger.LogWarning("[AviEconomyHook] Methods obtained.");

                #endregion

                #region Event

                Logger.LogWarning("[AviEconomyHook] Obtaining events...");
                var onBalanceChanged =
                    _bankType?.GetField("OnBalanceChanged", BindingFlags.Static | BindingFlags.NonPublic);
                var balanceManagerOnBalanceChangedMethod =
                    typeof(AviEconomyHook).GetMethod("ReflectOnBalanceChanged",
                        BindingFlags.Static | BindingFlags.NonPublic);
                if (onBalanceChanged == null || balanceManagerOnBalanceChangedMethod == null)
                    Logger.LogError("[AviEconomyHook] OnBalanceChanged event couldn't be loaded...");
                if (onBalanceChanged?.FieldType != null)
                {
                    if (balanceManagerOnBalanceChangedMethod is not null)
                    {
                        var balanceChanged =
                            Delegate.CreateDelegate(onBalanceChanged.FieldType, null, balanceManagerOnBalanceChangedMethod);
                        onBalanceChanged.SetValue(null, balanceChanged);
                    }
                }

                Logger.LogWarning("[AviEconomyHook] Events obtained.");

                #endregion

                Logger.LogWarning("[AviEconomyHook] AviEconomy hook loaded.");
                _initialized = true;
            }
            catch (Exception e)
            {
                Logger.LogError($"[AviEconomyHook] Failed to hook AviEconomy: {e.Message}");
                Logger.LogError($"[AviEconomyHook] Details: {e}");
            }
        }

        public static decimal? Withdraw(UnturnedPlayer? player, decimal amount)
        {
            return Deposit(player, -amount);
        }

        #endregion

        private static bool _initialized;
        private static Type? _bankType;
        private static MethodInfo? _getBalanceMethod;
        private static MethodInfo? _payAsServerMethod;

        public static decimal InitialBalance { get; set; }
        public static bool XpMode { get; set; }
        public static bool XpModeUseInitialBalance { get; set; }
        public static string? CurrencySymbol { get; set; }
        public static string? CurrencyName { get; set; }
        public static bool CurrencySymbolBeforeValue { get; set; }
        public static float SalaryInterval { get; set; }
        public static float ExperienceExchangeRate { get; set; }
        public static float MoneyExchangeRate { get; set; }
        public static decimal RewardForZombieHead { get; set; }
        public static decimal RewardForMegaZombieHead { get; set; }
        public static decimal MaxPaidZombiesPerHour { get; set; }
        public static decimal MaxPaidZombiesPerDay { get; set; }
        public static bool ShowZombieRewardMessages { get; set; }
        public static bool ShowZombieOverLimitMessages { get; set; }
        public static decimal RewardForPlayerHead { get; set; }
        public static decimal PenaltyForDeath { get; set; }
        internal static string? PenaltyForSuicide { get; set; }
        public static ushort BalanceBgEffectId { get; set; }
        public static ushort BalanceFgEffectId { get; set; }
        public static string? BalanceTextColor { get; set; }
        public static bool AddCurrencySymbolToUI { get; set; }
        public static bool UseOpenModEconomy { get; set; }
        public static string? MySqlConnectionString { get; set; }
        public static string? MySqlTableNamePrefix { get; set; }

        private static void ReflectOnBalanceChanged(string playerId, decimal oldAmount, decimal toValue)
        {
            OnBalanceChanged?.Invoke(playerId, oldAmount, toValue);
        }
    }
}
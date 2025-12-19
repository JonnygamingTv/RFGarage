using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Commands;
using RocketExtensions.Utilities;

namespace RocketExtensions.Models
{
    public static class CooldownManager
    {
        private static readonly Assembly AssemblyRocketCore = typeof(R).Assembly;
        private static readonly Type? TypeCooldown = AssemblyRocketCore.GetTypes().FirstOrDefault(x => x.Name == "RocketCommandCooldown"); // internal class
        private static readonly FieldInfo? FieldCooldowns = typeof(RocketCommandManager).GetField("cooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo? FieldCooldownPlayer = TypeCooldown?.GetField("Player", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo? FieldCooldownCommand = TypeCooldown?.GetField("Command", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo? FieldCooldownPermission = TypeCooldown?.GetField("ApplyingPermission", BindingFlags.Public | BindingFlags.Instance);
        private static IList? Cooldowns => FieldCooldowns?.GetValue(R.Commands) as IList;

        private static IRocketPlayer? GetPlayer(object cooldown) => FieldCooldownPlayer?.GetValue(cooldown) as IRocketPlayer;

        private static IRocketCommand? GetCommand(object cooldown) => FieldCooldownCommand?.GetValue(cooldown) as IRocketCommand;

        private static void SetPermission(object cooldown, Permission perm) => FieldCooldownPermission?.SetValue(cooldown, perm);

        /// <summary>
        /// Cancels a cooldown.
        /// Should be called from the game thread.
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <returns>True if the cooldown was found and canceled</returns>
        public static bool CancelCooldown(IRocketPlayer player, IRocketCommand command)
        {
            var cooldowns = Cooldowns;
            if (cooldowns != null)
                lock (cooldowns)
                {
                    object? cooldown = null;
                    foreach (var cd in cooldowns)
                    {
                        if (cd == null)
                            continue;

                        var cmd = GetCommand(cd);
                        var rocketPlayer = GetPlayer(cd);
                        if (cmd == null || rocketPlayer == null)
                            continue;
                        
                        if (cmd.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase) &&
                            rocketPlayer.Id.Equals(player.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            cooldown = cd;
                            break;
                        }
                    }

                    if (cooldown == null)
                    {
                        return false;
                    }

                    cooldowns.Remove(cooldown);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Cancels a cooldown.
        /// Can be called from any thread.
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <returns>True if the cooldown was found and canceled</returns>
        public static async Task<bool> CancelCooldownAsync(IRocketPlayer player, IRocketCommand command) =>
            await ThreadTool.RunOnGameThreadAsync(CancelCooldown, player, command);

        /// <summary>
        /// Sets a cooldown for a command.
        /// To cancel a cooldown, use <see cref="CancelCooldown(IRocketPlayer, IRocketCommand)"/> instead.
        /// Should be called from the game thread
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <param name="cooldown">Cooldown in seconds</param>
        /// <returns>True if a new cooldown was created, or false if an existing cooldown was updated</returns>
        public static bool SetCooldown(IRocketPlayer player, IRocketCommand command, uint cooldown)
        {
            var perm = new Permission(command.Name, cooldown);

            // check for an active cooldown

            var cooldowns = Cooldowns;
            if (cooldowns != null)
                lock (cooldowns)
                {
                    foreach (var cd in cooldowns)
                    {
                        if (cd == null)
                            continue;

                        var cmd = GetCommand(cd);
                        var rocketPlayer = GetPlayer(cd);
                        if (cmd == null || rocketPlayer == null)
                            continue;
                        
                        if (cmd.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase) &&
                            rocketPlayer.Id.Equals(player.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            // Found existing cooldown
                            SetPermission(cd, perm);
                            return false;
                        }
                    }

                    // No existing cooldown, create new
                    if (TypeCooldown != null)
                    {
                        var cdInst = Activator.CreateInstance(TypeCooldown, player, command, perm);
                        cooldowns.Add(cdInst);
                    }

                    return true;
                }

            return false;
        }

        public static async Task<bool> SetCooldownAsync(IRocketPlayer player, IRocketCommand command, uint cooldown) =>
            await ThreadTool.RunOnGameThreadAsync(SetCooldown, player, command, cooldown);
    }
}
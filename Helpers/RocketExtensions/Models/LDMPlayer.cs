using System;
using System.Linq;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers;
using RFRocketLibrary.Models;
using RFRocketLibrary.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;
using RocketExtensions.Utilities;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Color = UnityEngine.Color;
using SteamGroup = RFRocketLibrary.Models.SteamGroup;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Provides async methods to interact with a player from any thread.
    /// </summary>
    public class LDMPlayer
    {
        public LDMPlayer(IRocketPlayer player)
        {
            RocketPlayer = player ??
                           throw new ArgumentNullException($"{nameof(player)}", "IRocketPlayer player cannot be null");
        }

        internal LDMPlayer(Player player) : this(UnturnedPlayer.FromPlayer(player))
        {
        }

        /// <summary>
        /// The Player's RocketPlayer instance
        /// </summary>
        public IRocketPlayer RocketPlayer { get; }

        /// <summary>
        /// The Player's display name, or Console
        /// </summary>
        public string DisplayName => UnturnedPlayer?.DisplayName ?? "Console";

        /// <summary>
        /// Wether the Player's is Admin or not
        /// </summary>
        public bool IsAdmin => RocketPlayer.IsAdmin;

        public static LDMPlayer FromRocketPlayer(IRocketPlayer player) => new(player);
        public static LDMPlayer FromPlayer(Player player) => new(player);
        public static LDMPlayer FromSteamPlayer(SteamPlayer player) => new(player.player);
        public static LDMPlayer FromName(string player) => new(UnturnedPlayer.FromName(player));

        public bool IsConsole => RocketPlayer is ConsolePlayer || RocketPlayer.IsAdmin && RocketPlayer.Id == "0";

        /// <summary>
        /// The Player's position, or 0
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (Player != null && Player.transform != null)
                    return Player.transform.position;

                return Vector3.zero;
            }
        }

        /// <summary>
        /// The Player's rotation, or 0
        /// </summary>
        public float Rotation
        {
            get
            {
                if (Player != null && Player.transform != null)
                    return Player.transform.rotation.eulerAngles.y;

                return 0;
            }
        }

        /// <summary>
        /// The Player's LocationNode, or null
        /// </summary>
        public LocationNode? GetLocation() => LevelNodes.nodes.OfType<LocationNode>()
            .OrderBy(k => (k.point - Position).sqrMagnitude).FirstOrDefault();

        /// <summary>
        /// The Player's current vehicle, or null
        /// </summary>
        public InteractableVehicle? CurrentVehicle => UnturnedPlayer?.CurrentVehicle;

        /// <summary>
        /// The Player's CSteamID, or CSteamID.Nil
        /// </summary>
        public CSteamID CSteamID => UnturnedPlayer?.CSteamID ?? CSteamID.Nil;

        /// <summary>
        /// The Player's Group ID, or 0
        /// </summary>
        public ulong GroupID => Player != null ? Player.quests.groupID.m_SteamID : 0;

        /// <summary>
        /// The Player's Steam 64 ID, or 0
        /// </summary>
        public ulong PlayerID => UnturnedPlayer?.CSteamID.m_SteamID ?? 0;

        /// <summary>
        /// The Player's Transport Connection, or null
        /// </summary>
        public ITransportConnection? TransportConnection => SteamPlayer?.transportConnection;

        /// <summary>
        /// Player as UnturnedPlayer, or null
        /// </summary>
        public UnturnedPlayer? UnturnedPlayer => RocketPlayer as UnturnedPlayer;

        /// <summary>
        /// The Player, or null
        /// </summary>
        public Player? Player => UnturnedPlayer?.Player;

        /// <summary>
        /// The SteamPlayer, or null
        /// </summary>
        public SteamPlayer? SteamPlayer
        {
            get
            {
                if (Player != null)
                    return Player.channel.owner;

                return null;
            }
        }

        /// <summary>
        /// The Player's Steam Group CSteamID, or 0
        /// </summary>
        public ulong SteamGroupID => UnturnedPlayer?.SteamGroupID.m_SteamID ?? 0;

        /// <summary>
        /// Get Steam Profile, or null
        /// </summary>
        public async Task<SteamProfile?> GetSteamProfileAsync() => await SteamUtil.GetSteamProfileAsync(PlayerID);

        /// <summary>
        /// Get Steam Group, or null
        /// </summary>
        public async Task<SteamGroup?> GetSteamGroupAsync() => await SteamUtil.GetSteamGroupAsync(SteamGroupID);

        /// <summary>
        /// Sends a message to the player
        /// </summary>
        public async Task MessageAsync(string message, Color? messageColor = null, string? iconUrl = null)
        {
            messageColor ??= Color.green;
            message = message.ReformatColor();
            await ThreadTool.RunOnGameThreadAsync(ChatHelper.Say, RocketPlayer, message, messageColor, iconUrl);
        }

        /// <summary>
        /// Cancels the command cooldown for a command.
        /// </summary>
        /// <returns>True if the cooldown was found and cancelled</returns>
        public async Task<bool> CancelCooldownAsync(IRocketCommand command)
        {
            return await CooldownManager.CancelCooldownAsync(RocketPlayer, command);
        }

        /// <summary>
        /// Sets a command cooldown for a command
        /// </summary>
        /// <param name="command">Plugin command instance</param>
        /// <param name="cooldown">Cooldown time in seconds</param>
        /// <returns>True if a new cooldown was created, or false if an existing one was updated</returns>
        public async Task<bool> SetCooldownAsync(IRocketCommand command, uint cooldown)
        {
            return await CooldownManager.SetCooldownAsync(RocketPlayer, command, cooldown);
        }

        /// <summary>
        /// Gets the player's IP address, or 0.0.0.0
        /// </summary>
        public async Task<string?> GetIPAsync()
        {
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                var addressString = SteamPlayer?.getAddressString(false);
                return string.IsNullOrEmpty(addressString) ? "0.0.0.0" : addressString;
            });
        }

        public async Task TriggerEffectAsync(ushort effectID)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.TriggerEffect(effectID); });
        }

        public async Task MaxSkillsAsync()
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.MaxSkills(); });
        }

        /// <summary>
        /// Gives a player an item
        /// </summary>
        /// <returns>True if the item was given, otherwise false</returns>
        public async Task<bool> TryGiveItemAsync(ushort itemID, byte amount)
        {
            if (UnturnedPlayer == null)
                return false;

            return await ThreadTool.RunOnGameThreadAsync(() => UnturnedPlayer.GiveItem(itemID, amount));
        }

        /// <summary>
        /// Gives a player an item
        /// </summary>
        /// <returns>True if the item was given, otherwise false</returns>
        public async Task<bool> TryGiveItemAsync(Item item)
        {
            if (UnturnedPlayer == null)
                return false;

            return await ThreadTool.RunOnGameThreadAsync(() => UnturnedPlayer.GiveItem(item));
        }

        /// <summary>
        /// Tries to spawn a vehicle in front of the player
        /// </summary>
        /// <returns>True on vehicle spawned</returns>
        public async Task<bool> TryGiveVehicleAsync(ushort vehicleID)
        {
            if (UnturnedPlayer == null)
                return false;

            return await ThreadTool.RunOnGameThreadAsync(() => UnturnedPlayer.GiveVehicle(vehicleID));
        }

        public async Task KickAsync(string reason)
        {
            await ThreadTool.RunOnGameThreadAsync(Provider.kick, CSteamID, reason);
        }

        public async Task BanAsync(CSteamID instigator, string reason, uint duration)
        {
            if (UnturnedPlayer == null)
                return;
            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.Ban(instigator, reason, duration); });
        }

        public async Task BanAsync(string reason, uint duration) => await BanAsync(CSteamID.Nil, reason, duration);

        public async Task SetAdminAsync(bool admin)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.Admin(admin); });
        }

        public async Task SetAdminAsync(bool admin, UnturnedPlayer issuer)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.Admin(admin, issuer); });
        }

        public async Task SetAdminAsync(bool admin, LDMPlayer issuer)
        {
            if (UnturnedPlayer == null || issuer.UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.Admin(admin, issuer.UnturnedPlayer); });
        }

        /// <summary>
        /// Teleports a player to another player
        /// </summary>
        /// <param name="target"></param>
        /// <param name="safe">Runs position checks when enabled. Disabling this will force the tp, even if it is into an object</param>
        /// <returns>True if the player was teleported (always true in unsafe mode)</returns>
        public async Task<bool> TeleportAsync(UnturnedPlayer target, bool safe = true)
        {
            if (Player == null)
                return false;

            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                if (safe)
                    return Player.teleportToLocation(target.Position, target.Rotation);

                Player.teleportToLocationUnsafe(target.Position, target.Rotation);
                return true;
            });
        }

        /// <summary>
        /// Teleports a player to a position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="safe">Runs position checks when enabled. Disabling this will force the tp, even if it is into an object</param>
        /// <returns>True if the player was teleported (always true in unsafe mode)</returns>
        public async Task<bool> TeleportAsync(Vector3 position, float rotation, bool safe = true)
        {
            if (Player == null)
                return false;

            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                if (safe)
                    return Player.teleportToLocation(position, rotation);

                Player.teleportToLocationUnsafe(position, rotation);
                return true;
            });
        }

        public async Task HealAsync(byte amount, bool healBleeding = true, bool healBroken = true)
        {
            if (Player == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { Player.life.askHeal(amount, healBleeding, healBroken); });
        }

        public async Task HealAsync()
        {
            if (Player == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                Player.life.askHeal(100, true, true);
                Player.life.serverModifyFood(100);
                Player.life.serverModifyStamina(100);
                Player.life.serverModifyVirus(100);
                Player.life.serverModifyWater(100);
            });
        }

        public async Task<EPlayerKill> DamageAsync(byte amount, Vector3 direction, EDeathCause cause, ELimb limb,
            CSteamID damageDealer)
        {
            if (UnturnedPlayer == null)
                return EPlayerKill.NONE;

            return await ThreadTool.RunOnGameThreadAsync(() =>
                UnturnedPlayer.Damage(amount, direction, cause, limb, damageDealer));
        }

        public async Task SetSkillAsync(UnturnedSkill skill, byte level)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => { UnturnedPlayer.SetSkillLevel(skill, level); });
        }

        public async Task SendBrowserRequestAsync(string url, string message)
        {
            if (Player == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => Player.sendBrowserRequest(message, url));
        }

        /// <summary>
        /// Relays a player to another server
        /// </summary>
        public async Task RelayAsync(uint ip, ushort port, string password, bool showMenu = true)
        {
            if (Player == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => Player.sendRelayToServer(ip, port, password, showMenu));
        }

        /// <summary>
        /// Relays a player to another server
        /// </summary>
        public async Task RelayAsync(string ip, ushort port, string password, bool showMenu = true)
        {
            if (Player == null)
                return;

            var strings = ip.Split('.').ToArray();
            if (strings.Length != 4)
                throw new InvalidCastException($"Cannot convert ip '{ip}' into a numeric IP address");

            uint ipAddress;
            try
            {
                var b1 = byte.Parse(strings[0]);
                var b2 = byte.Parse(strings[1]);
                var b3 = byte.Parse(strings[2]);
                var b4 = byte.Parse(strings[3]);
                var buffer = new[] {b1, b2, b3, b4};
                ipAddress = BitConverter.ToUInt32(buffer, 0);
            }
            catch (FormatException)
            {
                throw new InvalidCastException($"Cannot convert ip '{ip}' into a numeric IP address");
            }

            await ThreadTool.RunOnGameThreadAsync(() => Player.sendRelayToServer(ipAddress, port, password, showMenu));
        }

        /// <summary>
        /// Toggles a plugin widget flag
        /// </summary>
        public async Task SetPluginWidgetFlagAsync(EPluginWidgetFlags flag, bool active)
        {
            if (Player == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() => Player.setPluginWidgetFlag(flag, active));
        }
    }
}
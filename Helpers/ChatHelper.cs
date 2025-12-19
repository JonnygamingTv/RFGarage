using System.Collections.Generic;
using System.Reflection;
using RFRocketLibrary.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFRocketLibrary.Helpers
{
    public static class ChatHelper
    {
        #region Methods

        public static void DisableMaxMessageLengthLimit()
        {
            typeof(ChatManager).GetField("MAX_MESSAGE_LENGTH", BindingFlags.Public | BindingFlags.Static)?
                .SetValue(null, int.MaxValue);
        }

        public static void Broadcast(string text, Color? color = null, string? iconURL = null)
        {
            foreach (var message in WrapMessage(text))
            {
                Logger.Log($"Broadcast: {message}");
                ChatManager.serverSendMessage(message, color ?? Color.green, null, null, EChatMode.GLOBAL, iconURL, true);
            }
        }

        public static void Say(UnturnedPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            Say(player.Player.channel.owner, text, color, iconURL);
        }

        public static void Say(UnturnedPlayer sender, UnturnedPlayer receiver, string text, Color? color = null, string? iconURL = null)
        {
            Say(sender.Player.channel.owner, receiver.Player.channel.owner, text, color, iconURL);
        }

        public static void Say(Player player, string text, Color? color = null, string? iconURL = null)
        {
            Say(player.channel.owner, text, color, iconURL);
        }

        public static void Say(Player sender, Player receiver, string text, Color? color = null, string? iconURL = null)
        {
            Say(sender.channel.owner, receiver.channel.owner, text, color, iconURL);
        }

        public static void Say(SteamPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            Say(null, player, text, color, iconURL);
        }

        public static void Say(SteamPlayer? sender, SteamPlayer? receiver, string text, Color? color = null, string? iconURL = null)
        {
            foreach (var s in WrapMessage(text))
                ChatManager.serverSendMessage(s, color ?? Color.green, sender, receiver, receiver == null ? EChatMode.SAY : EChatMode.GLOBAL, iconURL,
                    true);
        }

        public static void Say(CSteamID steamID, string text, Color? color = null, string? iconURL = null)
        {
            if (steamID == CSteamID.Nil || steamID.m_SteamID == 0)
                return;
            
            var steamPlayer = PlayerTool.getSteamPlayer(steamID);
            if (steamPlayer == null)
                return;
            
            Say(steamPlayer, text, color, iconURL);
        }

        public static void Say(CSteamID sender, CSteamID receiver, string text, Color? color = null, string? iconURL = null)
        {
            if (sender == CSteamID.Nil || sender.m_SteamID == 0)
                return;
            
            if (receiver == CSteamID.Nil || receiver.m_SteamID == 0)
                return;
            
            var senderPlayer = PlayerTool.getSteamPlayer(sender);
            if (senderPlayer == null)
                return;
            
            var receiverPlayer = PlayerTool.getSteamPlayer(receiver);
            if (receiverPlayer == null)
                return;
            
            Say(senderPlayer, receiverPlayer, text, color, iconURL);
        }

        public static void Say(IRocketPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            if (player is ConsolePlayer)
            {
                Logger.Log(text);
                return;
            }

            if (player is UnturnedPlayer uPlayer)
            {
                Say(uPlayer, text, color, iconURL);
                return;
            }

            if (!ulong.TryParse(player.Id, out var steamId))
                return;
            
            Say(new CSteamID(steamId), text, color, iconURL);
        }

        public static void Say(IRocketPlayer sender, IRocketPlayer receiver, string text, Color? color = null, string? iconURL = null)
        {
            if (sender is UnturnedPlayer senderU && receiver is UnturnedPlayer receiverU)
            {
                Say(senderU, receiverU, text, color, iconURL);
                return;
            }
            
            if (!ulong.TryParse(sender.Id, out var senderId))
                return;
            
            if (!ulong.TryParse(receiver.Id, out var receiverId))
                return;
            
            Say(new CSteamID(senderId), new CSteamID(receiverId), text, color, iconURL);
        }

        public static IEnumerable<string> WrapMessage(string text)
        {
            if (text.Length == 0) 
                return new List<string>();
            
            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = string.Empty;
            var cleanLength = 0;
            const int maxLength = 120;
            foreach (var currentWord in words)
            {
                var cleanWord = currentWord.RemoveRichTag();
                if (cleanLength > maxLength ||
                    cleanLength + cleanWord.Length > maxLength)
                {
                    lines.Add(currentLine);
                    currentLine = string.Empty;
                    cleanLength = 0;
                }
                
                if (cleanLength > 0)
                {
                    currentLine += " " + currentWord;
                    cleanLength += 1;
                }
                else
                    currentLine += currentWord;
                
                cleanLength += cleanWord.Length;
            }
  
            if (cleanLength > 0)
                lines.Add(currentLine);
            
            return lines;
        }

        #endregion
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
// using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RFRocketLibrary.Enum;
using RFRocketLibrary.Models;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Utils
{
    public static class SteamUtil
    {
        #region Methods

        public static async Task<SteamWebPlayer?> GetWebPlayerAsync(string apiKey, ulong steamId)
        {
            if (string.IsNullOrEmpty(apiKey))
                return null;

            using var web = new WebClient();
            web.Proxy = null;
            var result = await web.DownloadStringTaskAsync(
                $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steamId}");
            var deserialized = JsonConvert.DeserializeObject<SteamWebApi>(result);
            return deserialized?.response?.players?.FirstOrDefault(k => k.steamid == steamId.ToString());
        }

        public static async Task<string?> GetAvatarHashAsync(string apiKey, ulong steamId)
        {
            var steamWebPlayerAsync = await GetWebPlayerAsync(apiKey, steamId);
            return steamWebPlayerAsync?.avatarhash;
        }

        public static async Task<string> GetAvatarAsync(string apiKey, ulong steamId)
        {
            var avatarHash = await GetAvatarHashAsync(apiKey, steamId);
            return string.IsNullOrWhiteSpace(avatarHash)
                ? string.Empty
                : $"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/00/{avatarHash}.jpg";
        }

        public static string GetAvatar(ulong steamId, EAvatarProvider avatarProvider = EAvatarProvider.SteamIDXYZ)
        {
            try
            {
                switch (avatarProvider)
                {
                    case EAvatarProvider.SteamCommunity:
                    {
                        const string avatarNode = "<avatarFull><![CDATA[";
                        using var web = new WebClient();
                        web.Proxy = null;
                        var result = web.DownloadString($"https://steamcommunity.com/profiles/{steamId}?xml=1");
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("]]></avatarFull>", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        return result;
                    }
                    case EAvatarProvider.SteamIDXYZ:
                    {
                        const string avatarNode = "<img class=\"avatar\" src=\"";
                        using var web = new WebClient();
                        web.Proxy = null;
                        web.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0"; 
                        var result = web.UploadString($"https://steamid.xyz/{steamId}", "POST", string.Empty);
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("\">", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        return result;
                    }
                    case EAvatarProvider.SteamIDFinder:
                    {
                        const string avatarNode = "<img class=\"img-rounded avatar\" src=\"";
                        using var web = new WebClient();
                        web.Proxy = null;
                        var result = web.DownloadString($"https://www.steamidfinder.com/lookup/{steamId}");
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("\" alt", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        result = result.Replace("medium", "full");
                        return result;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(avatarProvider), avatarProvider, null);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<string> GetAvatarAsync(ulong steamId, EAvatarProvider avatarProvider = EAvatarProvider.SteamIDXYZ)
        {
            try
            {
                switch (avatarProvider)
                {
                    case EAvatarProvider.SteamCommunity:
                    {
                        const string avatarNode = "<avatarFull><![CDATA[";
                        using var web = new WebClient();
                        web.Proxy = null;
                        var result = await web.DownloadStringTaskAsync($"https://steamcommunity.com/profiles/{steamId}?xml=1");
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("]]></avatarFull>", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        return result;
                    }
                    case EAvatarProvider.SteamIDXYZ:
                    {
                        const string avatarNode = "<img class=\"avatar\" src=\"";
                        using var web = new WebClient();
                        web.Proxy = null;
                        web.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0"; 
                        var result = await web.UploadStringTaskAsync($"https://steamid.xyz/{steamId}", "POST", string.Empty);
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("\">", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        return result;
                    }
                    case EAvatarProvider.SteamIDFinder:
                    {
                        const string avatarNode = "<img class=\"img-rounded avatar\" src=\"";
                        using var web = new WebClient();
                        web.Proxy = null;
                        var result = await web.DownloadStringTaskAsync($"https://www.steamidfinder.com/lookup/{steamId}");
                        if (string.IsNullOrWhiteSpace(result))
                            return string.Empty;
                
                        var start = result.IndexOf(avatarNode, 0, StringComparison.Ordinal);
                        var end = result.IndexOf("\" alt", start, StringComparison.Ordinal);
                        result = result.Remove(end).Substring(start + avatarNode.Length);
                        result = result.Replace("medium", "full");
                        return result;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(avatarProvider), avatarProvider, null);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetGroupName(ulong groupId)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load("https://steamcommunity.com/gid/" + groupId + "/memberslistxml/?xml=1&p=1");
                var node = xml.SelectSingleNode("//*[local-name()='groupName']");
                return node?.InnerText ?? string.Empty;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetGroupName: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return string.Empty;
            }
        }

        public static SteamGroup? GetSteamGroup(ulong groupId, uint page = 1, bool useNode = false)
        {
            if (groupId == 0)
                return null;

            var url = $"https://steamcommunity.com/gid/{groupId}/memberslistxml/?xml=1&p={page}";
            switch (useNode)
            {
                case true:
                    var xml = new XmlDocument();
                    try
                    {
                        xml.Load(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using XmlReader reader = new XmlNodeReader(xml);
                        return (SteamGroup) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroup: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }

                case false:
                    var wc = new WebClient();
                    wc.Proxy = null;
                    try
                    {
                        var doc = wc.DownloadString(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using var reader = new StringReader(doc);
                        return (SteamGroup) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroup: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }
            }
        }

        public static async Task<SteamGroup?> GetSteamGroupAsync(ulong groupId)
        {
            var url = $"https://steamcommunity.com/gid/{groupId}/memberslistxml/?xml=1&p=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamGroup) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroupAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static string GetSteamName(ulong steamId)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load("https://steamcommunity.com/profiles/" + steamId + "?xml=1");
                var node = xml.SelectSingleNode("//*[local-name()='steamID']");
                return node?.InnerText ?? string.Empty;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamName: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return string.Empty;
            }
        }

        public static SteamProfile? GetSteamProfile(ulong steamId, bool useNode = false)
        {
            var url = $"https://steamcommunity.com/profiles/{steamId}?xml=1";
            switch (useNode)
            {
                case true:
                    var xml = new XmlDocument();
                    try
                    {
                        xml.Load(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using XmlReader reader = new XmlNodeReader(xml);
                        return (SteamProfile) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfile: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }

                case false:
                    var wc = new WebClient();
                    wc.Proxy = null;
                    try
                    {
                        var doc = wc.DownloadString(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using var reader = new StringReader(doc);
                        return (SteamProfile) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfile: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }
            }
        }

        public static async Task<SteamProfile?> GetSteamProfileAsync(ulong steamId)
        {
            if (steamId == 0)
                return null;

            var url = $"https://steamcommunity.com/profiles/{steamId}?xml=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamProfile) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfileAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        #endregion
    }
}
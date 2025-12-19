using System;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class SteamWebApi
    {
        public SteamWebApiResponse? response { get; set; }

        public SteamWebApi()
        {
            
        }
    }
    
    [Serializable]
    public class SteamWebApiResponse
    {
        public SteamWebPlayer[]? players { get; set; }

        public SteamWebApiResponse()
        {
            
        }
    }
    
    [Serializable]
    public class SteamWebPlayer
    {
        public string? steamid { get; set; }
        public int communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        public string? personaname { get; set; }
        public int commentpermission { get; set; }
        public string? profileurl { get; set; }
        public string? avatar { get; set; }
        public string? avatarmedium { get; set; }
        public string? avatarfull { get; set; }
        public string? avatarhash { get; set; }
        public int lastlogoff { get; set; }
        public int personastate { get; set; }
        public string? realname { get; set; }
        public string? primaryclanid { get; set; }
        public int timecreated { get; set; }
        public int personastateflags { get; set; }

        public SteamWebPlayer()
        {
            
        }
    }
}
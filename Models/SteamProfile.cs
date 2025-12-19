using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RFRocketLibrary.Models
{
    [Serializable]
    [XmlType("profile")]
    public class SteamProfile
    {
        [XmlElement("steamID64")]
        public ulong SteamID64 { get; set; }
        [XmlElement("steamID")]
        public string SteamID { get; set; } = string.Empty;
        [XmlElement("onlineState")]
        public string OnlineState { get; set; } = string.Empty;
        [XmlElement("stateMessage")]
        public string StateMessage { get; set; } = string.Empty;
        [XmlElement("privacyState")]
        public string PrivacyState { get; set; } = string.Empty;
        [XmlElement("visibilityState")]
        public int VisibilityState { get; set; }
        [XmlElement("avatarIcon")]
        public string AvatarIcon { get; set; } = string.Empty;
        [XmlElement("avatarMedium")]
        public string AvatarMedium { get; set; } = string.Empty;
        [XmlElement("avatarFull")]
        public string AvatarFull { get; set; } = string.Empty;
        [XmlElement("vacBanned")]
        public int VacBanned { get; set; }
        [XmlElement("tradeBanState")]
        public string TradeBanState { get; set; } = string.Empty;
        [XmlElement("isLimitedAccount")]
        public int IsLimitedAccount { get; set; }
        [XmlElement("customURL")]
        public string CustomURL { get; set; } = string.Empty;
        [XmlElement("memberSince")]
        public string MemberSince { get; set; } = string.Empty;
        [XmlElement("steamRating")]
        public int SteamRating { get; set; }
        [XmlElement("hoursPlayed2Weeks")]
        public float HoursPlayed2Weeks { get; set; }
        [XmlElement("headline")]
        public string Headline { get; set; } = string.Empty;
        [XmlElement("location")]
        public string Location { get; set; } = string.Empty;
        [XmlElement("realname")]
        public string RealName { get; set; } = string.Empty;
        [XmlElement("summary")] 
        public string Summary { get; set; } = string.Empty;
        [XmlArray("groups"), XmlArrayItem("group")] 
        public List<SteamProfileGroup> Groups { get; set; } = new();

        public SteamProfile()
        {
            
        }
    }
}
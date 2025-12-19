using System;
using System.Xml.Serialization;

namespace RFRocketLibrary.Models
{
    [Serializable]
    [XmlType("group")]
    public class SteamProfileGroup
    {
        [XmlAttribute("isPrimary")]
        public int IsPrimary { get; set; }
        [XmlElement("groupID64")]
        public ulong GroupID64 { get; set; }
        [XmlElement("groupName")]
        public string GroupName { get; set; } = string.Empty;
        [XmlElement("groupURL")]
        public string GroupURL { get; set; } = string.Empty;
        [XmlElement("headline")]
        public string Headline { get; set; } = string.Empty;
        [XmlElement("summary")]
        public string Summary { get; set; } = string.Empty;
        [XmlElement("avatarIcon")]
        public string AvatarIcon { get; set; } = string.Empty;
        [XmlElement("avatarMedium")]
        public string AvatarMedium { get; set; } = string.Empty;
        [XmlElement("avatarFull")]
        public string AvatarFull { get; set; } = string.Empty;
        [XmlElement("memberCount")]
        public int MemberCount { get; set; }
        [XmlElement("membersInChat")]
        public int MembersInChat { get; set; }
        [XmlElement("membersInGame")]
        public int MembersInGame { get; set; }
        [XmlElement("membersOnline")]
        public int MembersOnline { get; set; }

        public SteamProfileGroup()
        {
            
        }
    }
}
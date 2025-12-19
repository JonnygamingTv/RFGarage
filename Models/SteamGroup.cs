using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RFRocketLibrary.Models
{
    [Serializable]
    [XmlType("memberList")]
    public class SteamGroup
    {
        [XmlElement("groupID64")]
        public ulong GroupID64 { get; set; }
        [XmlElement("groupDetails")]
        public SteamGroupDetails? SteamGroupDetails { get; set; }
        [XmlElement("memberCount")]
        public int MemberCount { get; set; }
        [XmlElement("totalPages")]
        public int TotalPages { get; set; }
        [XmlElement("currentPage")]
        public int CurrentPage { get; set; }
        [XmlElement("startingMember")]
        public int StartingMember { get; set; }
        [XmlElement("previousPageLink")]
        public string PreviousPageLink { get; set; } = string.Empty;
        [XmlElement("nextPageLink")]
        public string NextPageLink { get; set; } = string.Empty;
        [XmlArray("members"), XmlArrayItem("steamID64")]
        public List<ulong> Members { get; set; } = new();

        public SteamGroup()
        {
            
        }
    }
}
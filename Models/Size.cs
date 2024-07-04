using System.Collections.Generic;
using System.Xml.Serialization;
using RFGarage.Enums;

namespace RFGarage.Models
{
    public class Size
    {
        [XmlAttribute]
        public string Perm;
        [XmlAttribute]
        public byte size;

        public Size(){}
        public Size(string p, byte s) { Perm = p; size = s; }
    }
}

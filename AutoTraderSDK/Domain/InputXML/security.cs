using System.Xml.Serialization;
using AutoTraderSDK.Domain.OutputXML;

namespace AutoTraderSDK.Domain.InputXML
{
    public class security
    {
        [XmlAttribute]
        public int secid { get; set; }

        [XmlAttribute]
        public string active { get; set; }

        [XmlElement(IsNullable = false)]
        public string seccode { get; set; }

        [XmlElement(IsNullable = false)]
        public string instrclass { get; set; }

        [XmlElement(IsNullable = false)]
        public string currency { get; set; }

        [XmlElement(IsNullable = false)]
        public string board { get; set; }

        [XmlElement(IsNullable = false)]
        public int market { get; set; }

        [XmlElement(IsNullable = false)]
        public string shortname { get; set; }

        [XmlElement(IsNullable = false)]
        public int decimals { get; set; }

        [XmlElement(IsNullable = false)]
        public double minstep { get; set; }

        [XmlElement(IsNullable = false)]
        public int lotsize { get; set; }

        [XmlElement(IsNullable = false)]
        public double point_cost { get; set; }

        [XmlElement(IsNullable = false)]
        public opmask opmask { get; set; }

        [XmlElement(IsNullable = false)]
        public string sectype { get; set; }

        [XmlElement(IsNullable = false)]
        public string sec_tz { get; set; }

        [XmlElement(IsNullable = false)]
        public string quotestype { get; set; }
    }
}

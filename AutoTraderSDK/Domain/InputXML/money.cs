using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class money
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlElement]
        public double open_balance { get; set; }

        [XmlElement]
        public double bought { get; set; }

        [XmlElement]
        public double sold { get; set; }

        [XmlElement]
        public double balance { get; set; }

        [XmlElement]
        public double settled { get; set; }

        [XmlElement]
        public double tax { get; set; }

        [XmlElement("value_part")]
        public List<value_part> value_parts { get; set; }
    }
}

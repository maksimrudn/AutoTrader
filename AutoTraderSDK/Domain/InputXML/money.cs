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


        [XmlAttribute]
        public string currency { get; set; }


        [XmlElement]
        public double balance_prc { get; set; }

        [XmlElement]
        public double open_balance { get; set; }

        [XmlElement]
        public double bought { get; set; }

        [XmlElement]
        public double sold { get; set; }

        [XmlElement]
        public double balance { get; set; }


        [XmlElement]
        public double blocked { get; set; }

        [XmlElement]
        public double estimated { get; set; }


        [XmlElement]
        public double fee { get; set; }


        [XmlElement]
        public double vm { get; set; }

        [XmlElement]
        public double finres { get; set; }

        [XmlElement]
        public double cover { get; set; }


        [XmlElement]
        public double settled { get; set; }

        [XmlElement]
        public double tax { get; set; }

        [XmlElement("value_part")]
        public List<value_part> value_parts { get; set; }
    }
}

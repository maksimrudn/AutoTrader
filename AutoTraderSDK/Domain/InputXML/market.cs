using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class market
    {
        [XmlAttribute]
        public int id { get; set; }
        [XmlText]
        public string InnerText { get; set; }
    }
}

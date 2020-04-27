using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class result
    {
        [XmlAttribute]
        public bool success { get; set; }

        public string message { get; set; }

        [XmlAttribute]
        public int transactionid { get; set; }
    }
}

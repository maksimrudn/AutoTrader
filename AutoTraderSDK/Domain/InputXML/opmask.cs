using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class opmask
    {
        [XmlAttribute]
        public string usecredit { get; set; }
        [XmlAttribute]
        public string bymarket { get; set; }
        [XmlAttribute]
        public string nosplit { get; set; }
        [XmlAttribute]
        public string immorcancel { get; set; }
        [XmlAttribute]
        public string cancelbalance { get; set; }
    }
}

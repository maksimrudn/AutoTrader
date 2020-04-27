﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.OutputXML
{
    public class proxy
    {
        [XmlAttribute]
        public string type { get; set; }

        [XmlAttribute]
        public string addr { get; set; }

        [XmlAttribute]
        public string port { get; set; }

        [XmlAttribute]
        public string login { get; set; }

        [XmlAttribute]
        public string password { get; set; }
    }
}

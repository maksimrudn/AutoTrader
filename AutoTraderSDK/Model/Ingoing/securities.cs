﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    public class securities
    {
        [XmlElement("security")]
        public List<securities_ns.security> security { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    public class markets
    {
        [XmlElement("market")]
        public List<market> market { get; set; }
    }
}
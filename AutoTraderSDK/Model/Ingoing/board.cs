﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    public class board
    {
        [XmlAttribute]
        public int id { get; set; }

        public string name { get; set; }

        public string market { get; set; }

        public string type { get; set; }
    }
}
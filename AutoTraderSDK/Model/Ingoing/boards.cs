﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    [Serializable, XmlRoot("boards")]
    public class boards
    {
        [XmlElement("board")]
        public List<board> board { get; set; }
    }
}
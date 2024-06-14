﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class market
    {
        [XmlAttribute]
        public int id { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        [XmlText]
        public string InnerText { get; set; }
    }
}
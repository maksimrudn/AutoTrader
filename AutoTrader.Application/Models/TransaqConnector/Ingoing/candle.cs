﻿using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class candle
    {
        [XmlAttribute]
        public string date { get; set; }
        [XmlAttribute]
        public double open { get; set; }
        [XmlAttribute]
        public double high { get; set; }
        [XmlAttribute]
        public double low { get; set; }
        [XmlAttribute]
        public double close { get; set; }
        [XmlAttribute]
        public int volume { get; set; }
        [XmlAttribute]
        public int oi { get; set; }
    }
}

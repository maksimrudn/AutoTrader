using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    public class candles
    {
        [XmlAttribute]
        public int secid { get; set; }
        [XmlAttribute]
        public int period { get; set; }
        [XmlAttribute]
        public int status { get; set; }
        [XmlAttribute]
        public string board { get; set; }
        [XmlAttribute]
        public string seccode { get; set; }

        [XmlElement("candle")]
        public List<candle> candle { get; set; }
    }
}

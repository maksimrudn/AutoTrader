using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    //сделка
    public class trade
    {
        [XmlAttribute]
        public int transactionid { get; set; }

        public int secid { get; set; }

        public Int64 tradeno { get; set; }

        public Int64 orderno { get; set; }

        public string board { get; set; }

        public string seccode { get; set; }

        public string client { get; set; }

        public string buysell { get; set; }

        public string time { get; set; }

        public string brokerref { get; set; }

        public double value { get; set; }

        public double comission { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public Int64 items { get; set; }

        public double yield { get; set; }

        public Int64 currentpos { get; set; }

        public double accruedint { get; set; }

        public string tradetype { get; set; }

        public string settlecode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class result
    {
        [XmlAttribute]
        public bool success { get; set; }

        public string message { get; set; }

        [XmlAttribute]
        public int transactionid { get; set; }
    }
}

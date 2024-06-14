using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class server_status
    {
        [XmlAttribute]
        public string connected { get; set; }

        [XmlText]
        public string InnerText { get; set; }
    }
}

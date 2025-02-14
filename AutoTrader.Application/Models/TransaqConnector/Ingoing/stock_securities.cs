using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    [XmlRoot("securities", Namespace = "")]
    public class stock_securities
    {
        [XmlElement("security")]
        public List<securities_ns.stock_security> security { get; set; }
    }
}

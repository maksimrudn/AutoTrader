using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class orders
    {
        [XmlElement("order")]
        public List<orders_ns.order> order { get; set; }
    }
}

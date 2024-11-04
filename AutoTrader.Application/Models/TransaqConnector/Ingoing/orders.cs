using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class orders
    {
        [XmlElement("order")]
        public List<orders_ns.order> order { get; set; }
    }
}

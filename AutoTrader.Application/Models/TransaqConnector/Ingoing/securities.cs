using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class securities
    {
        [XmlElement("security")]
        public List<securities_ns.security> security { get; set; }
    }
}

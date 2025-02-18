using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class markets
    {
        [XmlElement("market")]
        public List<market> market { get; set; }
    }
}

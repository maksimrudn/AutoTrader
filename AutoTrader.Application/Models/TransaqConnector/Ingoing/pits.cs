using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class pits
    {
        [XmlElement("pit")]
        public List<pit> pit { get; set; }
    }
}

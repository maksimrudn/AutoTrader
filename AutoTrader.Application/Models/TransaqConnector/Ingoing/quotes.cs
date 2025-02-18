using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class quotes
    {
        [XmlElement("quote")]
        public List<quotes_ns.quote> quote { get; set; }
        
    }

}

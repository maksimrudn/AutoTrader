using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Outgoing
{
    public class proxy
    {
        [XmlAttribute]
        public string type { get; set; }

        [XmlAttribute]
        public string addr { get; set; }

        [XmlAttribute]
        public string port { get; set; }

        [XmlAttribute]
        public string login { get; set; }

        [XmlAttribute]
        public string password { get; set; }
    }
}

using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class board
    {
        [XmlAttribute]
        public int id { get; set; }

        public string name { get; set; }

        public string market { get; set; }

        public string type { get; set; }
    }
}

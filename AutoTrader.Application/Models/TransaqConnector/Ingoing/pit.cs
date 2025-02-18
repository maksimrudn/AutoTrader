using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class pit
    {
        [XmlAttribute]
        public string seccode { get; set; }
        [XmlAttribute]
        public string board { get; set; }

        public int market { get; set; }

        public int decimals { get; set; }

        public double minstep { get; set; }

        public int lotsize { get; set; }

        public double point_cost { get; set; }
    }
}

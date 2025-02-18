using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing.securities_ns
{
    public class opmask
    {
        /// <summary>
        /// yes/no
        /// </summary>
        [XmlAttribute]
        public string usecredit { get; set; }


        /// <summary>
        /// yes/no
        /// </summary>
        [XmlAttribute]
        public string bymarket { get; set; }

        /// <summary>
        /// yes/no
        /// </summary>
        [XmlAttribute]
        public string nosplit { get; set; }

        /// <summary>
        /// yes/no
        /// </summary>
        [XmlAttribute]
        public string fok { get; set; }


        /// <summary>
        /// yes/no
        /// </summary>
        [XmlAttribute]
        public string ioc { get; set; }


        [Obsolete]
        [XmlAttribute]
        public string immorcancel { get; set; }


        [Obsolete]
        [XmlAttribute]
        public string cancelbalance { get; set; }
    }
}

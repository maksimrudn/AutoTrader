using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing
{
    public class pits
    {
        [XmlElement("pit")]
        public List<pit> pit { get; set; }
    }
}

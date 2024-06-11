using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing
{
    [Serializable, XmlRoot("boards")]
    public class boards
    {
        [XmlElement("board")]
        public List<board> board { get; set; }
    }
}

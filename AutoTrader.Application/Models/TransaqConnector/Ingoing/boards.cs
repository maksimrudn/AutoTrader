using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    [Serializable, XmlRoot("boards")]
    public class boards
    {
        [XmlElement("board")]
        public List<board> board { get; set; }
    }
}

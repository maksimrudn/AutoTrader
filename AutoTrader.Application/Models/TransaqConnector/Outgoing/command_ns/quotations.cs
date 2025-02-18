using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Outgoing.command_ns
{
    /// <summary>
    ///  подписка на изменения показателей торгов
    /// </summary>
    public class quotations
    {
        public quotations()
        {
            security = new List<security>();
        }

        [XmlElement("security")]
        public List<security> security;
    }
}

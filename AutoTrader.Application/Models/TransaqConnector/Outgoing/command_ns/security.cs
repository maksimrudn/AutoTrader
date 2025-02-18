using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Outgoing.command_ns
{
    public class security
    {       

        /// <summary>
        /// Код инструмента
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string seccode { get; set; }


        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string board { get; set; }

    }
}

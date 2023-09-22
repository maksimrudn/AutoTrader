using System.Xml.Serialization;
using AutoTraderSDK.Model.Outgoing;

namespace AutoTraderSDK.Model.Outgoing.command_ns
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

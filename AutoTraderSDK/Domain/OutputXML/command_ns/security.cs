using System.Xml.Serialization;
using AutoTraderSDK.Domain.OutputXML;

namespace AutoTraderSDK.Domain.OutputXML.command_ns
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

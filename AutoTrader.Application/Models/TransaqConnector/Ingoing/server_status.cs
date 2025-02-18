using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class server_status
    {
        /// <summary>
        /// true/false/error 
        /// </summary>
        [XmlAttribute]
        public string connected { get; set; }

        /// <summary>
        /// ID сервера 
        /// </summary>
        [XmlAttribute] 
        public int id { get; set; }
        
        /// <summary>
        /// true/атрибут отсутствует
        /// </summary>
        [XmlAttribute] 
        public string recover { get; set; }
        
        /// <summary>
        /// имя таймзоны сервера 
        /// </summary>
        [XmlAttribute] 
        public string server_tz { get; set; }
        
        /// <summary>
        /// версия системы 
        /// </summary>
        [XmlAttribute] 
        public int sys_ver { get; set; }
        
        /// <summary>
        /// билд сервера 
        /// </summary>
        [XmlAttribute] 
        public int build { get; set; }

        [XmlText]
        public string InnerText { get; set; }
    }
}

using System.Xml.Serialization;
using AutoTraderSDK.Domain.OutputXML;

namespace AutoTraderSDK.Domain.InputXML.quotations_ns
{
    public class quotation
    {
        /// <summary>
        /// внутренний код
        /// </summary>
        [XmlAttribute]
        public int secid { get; set; }


        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string board { get; set; }


        /// <summary>
        /// Код инструмента
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string seccode { get; set; }


        /// <summary>
        /// НКД на дату торгов в расчете на одну бумагу, руб
        /// </summary>
        [XmlAttribute]
        public double accruedintvalue { get; set; }


        /// <summary>
        /// Средневзвешенная цена
        /// </summary>
        [XmlAttribute]
        public double waprice { get; set; }

    }
}

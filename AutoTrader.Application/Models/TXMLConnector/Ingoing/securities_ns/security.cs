using System.Xml.Serialization;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing.securities_ns
{
    public class security
    {
        /// <summary>
        /// внутренний код
        /// </summary>
        [XmlAttribute]
        public int secid { get; set; }

        /// <summary>
        /// true/false
        /// </summary>

        [XmlAttribute]
        public string active { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string seccode { get; set; }

        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string instrclass { get; set; }

        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string currency { get; set; }

        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string board { get; set; }

        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public int market { get; set; }

        /// <summary>
        /// Идентификатор режима торгов по умолчанию
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string shortname { get; set; }

        /// <summary>
        /// Количество десятичных знаков в цене
        /// </summary>
        [XmlElement(IsNullable = false)]
        public int decimals { get; set; }

        /// <summary>
        /// Шаг цены
        /// </summary>

        [XmlElement(IsNullable = false)]
        public double minstep { get; set; }

        /// <summary>
        /// Шаг цены
        /// </summary>

        [XmlElement(IsNullable = false)]
        public int lotsize { get; set; }

        /// <summary>
        /// Делитель лота
        /// </summary>

        [XmlElement(IsNullable = false)]
        public int lotdivider { get; set; }


        /// <summary>
        /// Стоимость пункта цены
        /// </summary>
        [XmlElement(IsNullable = false)]
        public double point_cost { get; set; }

        /// <summary>
        /// 
        /// </summary>

        [XmlElement(IsNullable = false)]
        public opmask opmask { get; set; }

        /// <summary>
        /// Тип бумаги
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string sectype { get; set; }

        /// <summary>
        /// имя таймзоны инструмента (типа "Russian Standard Time", "USA=Eastern Standard Time"), содержит секцию CDATA
        /// </summary>

        [XmlElement(IsNullable = false)]
        public string sec_tz { get; set; }


        /// <summary>
        /// 0 - без стакана; 1 - стакан типа OrderBook; 2 -стакан типа Level2
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string quotestype { get; set; }


        /// <summary>
        /// код биржи листинга по стандарту ISO<
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string MIC { get; set; }

        /// <summary>
        /// Валюта расчетов режима торгов по умолчанию
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string currencyid { get; set; }
    }
}

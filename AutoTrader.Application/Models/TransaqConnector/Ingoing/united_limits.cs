using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class united_limits
    {
        /// <summary>
        /// Код юниона
        /// </summary>
        [XmlAttribute]
        public string union { get; set; }

        /// <summary>
        /// Входящая оценка стоимости единого портфеля
        /// </summary>
        public double open_equity { get; set; }

        /// <summary>
        /// Текущая оценка стоимости единого портфеля
        /// </summary>
        public double equity { get; set; }

        /// <summary>
        /// Начальные требования
        /// </summary>
        public double requirements { get; set; }

        /// <summary>
        /// Свободные средства
        /// </summary>
        public double free { get; set; }

        /// <summary>
        /// Финансовый результат последнего клиринга FORTS
        /// </summary>
        public double finres { get; set; }

        /// <summary>
        /// Размер требуемого ГО, посчитанный биржей FORTS
        /// </summary>
        public double go { get; set; }
    }
}
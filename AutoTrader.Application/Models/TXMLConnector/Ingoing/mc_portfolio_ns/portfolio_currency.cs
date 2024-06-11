using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing.mc_portfolio_ns
{
    public class portfolio_currency
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        [XmlAttribute]
        public string currency { get; set; }

        /// <summary>
        /// Курс валюты
        /// </summary>
        [XmlElement("cross_rate")]
        public double cross_rate { get; set; }

        /// <summary>
        /// Входящая денежная позиция
        /// </summary>
        [XmlElement("open_balance")]
        public double open_balance { get; set; }

        /// <summary>
        /// Текущая денежная позиция
        /// </summary>
        [XmlElement("balance")]
        public double balance { get; set; }

        /// <summary>
        /// Оценка текущей стоимости
        /// </summary>
        [XmlElement("equity")]
        public double equity { get; set; }

        /// <summary>
        /// Оценка текущей стоимости
        /// </summary>
        [XmlElement("cover")]
        public double cover { get; set; }

        /// <summary>
        /// Плановый риск
        /// </summary>
        [XmlElement("init_req")]
        public double init_req { get; set; }

        /// <summary>
        /// Минимальные требования
        /// </summary>
        [XmlElement("maint_req")]
        public double maint_req { get; set; }

        /// <summary>
        /// Нереализованная прибыль/убыток
        /// </summary>
        [XmlElement("unrealized_pnl")]
        public double unrealized_pnl { get; set; }
    }
}

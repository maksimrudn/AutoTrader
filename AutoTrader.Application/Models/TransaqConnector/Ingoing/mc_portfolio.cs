using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    /// <summary>
    /// Клиентский мультивалютный портфель
    /// </summary>
    public class mc_portfolio
    {
        [XmlAttribute]
        public string union { get; set; }

        [XmlAttribute]
        public string client { get; set; }

        /// <summary>
        /// Входящая оценка стоимости единого портфеля
        /// </summary>
        [XmlElement]
        public double open_equity { get; set; }

        /// <summary>
        /// Текущая оценка стоимости единого портфеля
        /// </summary>
        [XmlElement]
        public double equity { get; set; }

        [Obsolete]
        [XmlElement]
        public double chrgoff_ir { get; set; }

        /// <summary>
        /// Плановый риск (размер начальных требований)
        /// </summary>
        [XmlElement]
        public double init_req { get; set; }

        [Obsolete]
        [XmlElement]
        public double chrgoff_mr { get; set; }


        /// <summary>
        /// Размер минимальных требований
        /// </summary>
        [XmlElement]
        public double maint_req { get; set; }

        [Obsolete]
        [XmlElement]
        public double vm { get; set; }

        [XmlElement]
        public double finres { get; set; }

        /// <summary>
        /// Размер требуемого ГО FORTS (рассчитанный биржей)
        /// </summary>
        [XmlElement]
        public double go { get; set; }

        [Obsolete]
        [XmlElement]
        public double vm_mma { get; set; }


        /// <summary>
        /// Прибыль/убыток общий
        /// </summary>

        [XmlElement("pl")]
        public double pl { get; set; }

        /// <summary>
        /// Плановый размер обеспечения
        /// </summary>

        [XmlElement("cover")]
        public double cover { get; set; }

        /// <summary>
        /// Нереализов. прибыль/убыток
        /// </summary>
        [XmlElement("unrealized_pnl")]
        public double unrealized_pnl { get; set; }



        [XmlElement("money")]
        public List<mc_portfolio_ns.money> moneys { get; set; }

        [XmlElement("asset")]
        public List<mc_portfolio_ns.asset> assets { get; set; }
        

        [XmlElement("portfolio_currency")]
        public List<mc_portfolio_ns.portfolio_currency> portfolio_currency { get; set; }


        [XmlElement("security")]
        public List<mc_portfolio_ns.security> securities { get; set; }

    }
}

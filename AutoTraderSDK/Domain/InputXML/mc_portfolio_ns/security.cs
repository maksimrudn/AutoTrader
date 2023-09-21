
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML.mc_portfolio_ns
{
    public class security
    {
        /// <summary>
        /// id инструмента
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// id рынка
        /// </summary>
        public int market { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>
        public string seccode { get; set; }

        /// <summary>
        /// Код базового актива
        /// </summary>
        public string asset { get; set; }

        /// <summary>
        /// Код валюты
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Входящая цена 
        /// </summary>
        public double price_in { get; set; }

        /// <summary>
        /// Текущая цена
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// Валюта цены
        /// </summary>
        public string price_currency { get; set; }

        /// <summary>
        /// Кросс-курс валюты цены к валюте риска
        /// </summary>
        public string price_currency_crossrate { get; set; }

        /// <summary>
        /// Валюта балансовой цены
        /// </summary>
        public string balance_price_currency { get; set; }

        /// <summary>
        /// Кросс-курс валюты балансовой цены к валюте риска
        /// </summary>
        public string balance_price_crossrate { get; set; }

        /// <summary>
        /// Входящая нетто-позиция, штук 
        /// </summary>
        public int open_balance { get; set; }

        /// <summary>
        /// Куплено, штук
        /// </summary>
        public int bought { get; set; }

        /// <summary>
        /// Продано, штук
        /// </summary>
        public int sold { get; set; }

        /// <summary>
        /// Текущая нетто-позиция, штук
        /// </summary>
        public int balance { get; set; }

        /// <summary>
        /// Балансовая цена
        /// </summary>
        public double balance_prc { get; set; }

        /// <summary>
        /// Нереализов. прибыль/убыток
        /// </summary>
        public double unrealized_pnl { get; set; }

        /// <summary>
        /// Заявлено купить, штук
        /// </summary>
        public int buying { get; set; }

        /// <summary>
        /// Заявлено продать, штук
        /// </summary>
        public int selling { get; set; }

        /// <summary>
        /// Оценка текущей стоимости
        /// </summary>
        public double equity { get; set; }

        /// <summary>
        /// Вклад в плановое обеспечение
        /// </summary>
        public double reg_equity { get; set; }

        /// <summary>
        /// Ставка риска для лонгов, %
        /// </summary>
        public double riskrate_long { get; set; }

        /// <summary>
        /// Ставка риска для шортов, %
        /// </summary>
        public double riskrate_short { get; set; }

        /// <summary>
        /// Ставка резерва для лонгов, %
        /// </summary>
        public double reserate_long { get; set; }

        /// <summary>
        /// Ставка резерва для шортов, %
        /// </summary>
        public double reserate_short { get; set; }

        /// <summary>
        /// Прибыль/убыток общий
        /// </summary>
        public double pl { get; set; }

        /// <summary>
        /// Прибыль/убыток по входящим позициям
        /// </summary>
        public double pnl_income { get; set; }

        /// <summary>
        /// Прибыль/убыток по сделкам
        /// </summary>
        public double pnl_intraday { get; set; }

        /// <summary>
        /// Максимальная покупка, в лотах
        /// </summary>
        public int maxbuy { get; set; }

        /// <summary>
        /// Макcимальная продажа, в лотах
        /// </summary>
        public int maxsell { get; set; }


        /// <summary>
        /// Индивидуальный шорт-лимит
        /// </summary>
        public int borrowed { get; set; }

        // todo: commented becouse of problem with attribute with same element's name in "money"
        //[XmlElement(ElementName = "value_part")]
        //public List<security_ns.value_part> value_parts { get; set; }


    }
}

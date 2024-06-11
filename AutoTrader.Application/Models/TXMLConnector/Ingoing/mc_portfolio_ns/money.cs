using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing.mc_portfolio_ns
{
    public class money
    {
        /// <summary>
        /// Наименование денежного раздела
        /// </summary>
        [XmlAttribute]
        public string name { get; set; }

        /// <summary>
        /// код валюты
        /// </summary>
        [XmlAttribute]
        public string currency { get; set; }

        /// <summary>
        /// Код базового актива
        /// </summary>
        [XmlAttribute]
        public string asset { get; set; }

        /// <summary>
        /// Балансовая цена инвалютной денежной позиции,руб
        /// </summary>
        [XmlElement]
        public double balance_prc { get; set; }

        /// <summary>
        /// Входящая денежная позиция
        /// </summary>
        [XmlElement]
        public double open_balance { get; set; }

        /// <summary>
        /// Затрачено на покупки
        /// </summary>
        [XmlElement]
        public double bought { get; set; }

        /// <summary>
        /// Выручено от продаж
        /// </summary>
        [XmlElement]
        public double sold { get; set; }

        /// <summary>
        /// Текущая денежная позиция
        /// </summary>
        [XmlElement]
        public double balance { get; set; }

        /// <summary>
        /// Сумма плановых покупок
        /// </summary>
        [XmlElement]
        public double blocked { get; set; }

        /// <summary>
        /// Сумма плановых продаж
        /// </summary>
        [XmlElement]
        public double estimated { get; set; }

        /// <summary>
        /// Удержано комиссии
        /// </summary>
        [XmlElement]
        public double fee { get; set; }

        /// <summary>
        /// Вариационная маржа  [FORTS или фьючерсы MMA]
        /// </summary>
        [XmlElement]
        public double vm { get; set; }

        /// <summary>
        /// Фин. результат последнего клиринга
        /// </summary>
        [XmlElement]
        public double finres { get; set; }

        /// <summary>
        /// Вклад в плановое обеспечение
        /// </summary>
        [XmlElement]
        public double cover { get; set; }

        [Obsolete]
        [XmlElement]
        public double settled { get; set; }

        [Obsolete]
        [XmlElement]
        public double tax { get; set; }


        [XmlElement(ElementName = "value_part")]
        public List<money_ns.value_part> value_parts { get; set; }
    }
}

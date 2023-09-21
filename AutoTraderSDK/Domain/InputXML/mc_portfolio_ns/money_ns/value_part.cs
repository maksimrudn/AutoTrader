using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML.mc_portfolio_ns.money_ns
{
    public class value_part
    {
        /// <summary>
        /// Регистр учёта
        /// </summary>
        [XmlAttribute]
        public string register { get; set; }

        /// <summary>
        /// Входящая денежная позиция
        /// </summary>
        [XmlElement]
        public double open_balance { get; set; }

        /// <summary>
        /// Затрачено на покупк
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
    }
}

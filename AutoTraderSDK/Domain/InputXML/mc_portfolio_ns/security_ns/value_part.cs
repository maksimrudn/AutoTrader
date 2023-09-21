using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML.mc_portfolio_ns.security_ns
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
        public int open_balance { get; set; }

        /// <summary>
        /// Затрачено на покупк
        /// </summary>
        [XmlElement]
        public int bought { get; set; }

        /// <summary>
        /// Выручено от продаж
        /// </summary>
        [XmlElement]
        public int sold { get; set; }

        /// <summary>
        /// Текущая денежная позиция
        /// </summary>
        [XmlElement]
        public int balance { get; set; }



        /// <summary>
        /// Заявлено купить, штук
        /// </summary>
        [XmlElement]
        public int buying { get; set; }

        /// <summary>
        /// Заявлено продать, штук
        /// </summary>
        [XmlElement]
        public int selling { get; set; }
    }
}

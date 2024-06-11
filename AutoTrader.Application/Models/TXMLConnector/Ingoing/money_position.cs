using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing
{
    public class money_position
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Идентификатор клиента 
        /// </summary>
        public string client { get; set; }

        /// <summary>
        /// Код юниона
        /// </summary>
        public string union { get; set; }


        [Obsolete]
        public int market { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        public List<market> markets { get; set; }

        /// <summary>
        /// Регистр учета
        /// </summary>
        public string register { get; set; }

        /// <summary>
        /// Код вида средств
        /// </summary>
        public string asset { get; set; }

        /// <summary>
        /// Наименование вида средств
        /// </summary>
        public string shortname { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        public double saldoin { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        public double bought { get; set; }

        /// <summary>
        /// Продано
        /// </summary>
        public double sold { get; set; }

        /// <summary>
        /// Текущее сальдо
        /// </summary>
        public double saldo { get; set; }

        /// <summary>
        /// В заявках на покупку + комиссия
        /// </summary>
        public double ordbuy { get; set; }

        /// <summary>
        /// В условных заявках на покупку
        /// </summary>
        public double ordbuycond { get; set; }

        /// <summary>
        /// В условных заявках на покупку
        /// </summary>
        public double commission { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing
{
    /// <summary>
    /// возвращается для фьючерсов, опционов, позиций по 
    /// инструментам РТС Стандарт.
    /// </summary>
    public class forts_money
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string client { get; set; }

        /// <summary>
        /// Код юниона
        /// </summary>
        public string union { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        public int market { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        public List<market> markets { get; set; }

        /// <summary>
        /// Наименование вида средств
        /// </summary>
        public string shortname { get; set; }

        /// <summary>
        /// Текущие
        /// </summary>
        public double current { get; set; }

        /// <summary>
        /// Заблокировано
        /// </summary>
        public double blocked { get; set; }

        /// <summary>
        /// Свободные
        /// </summary>
        public double free { get; set; }

        /// <summary>
        /// Опер. маржа
        /// </summary>
        public double varmargin { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class sec_position
    {
        public int secid { get; set; }

        public int market { get; set; }

        public string seccode { get; set; }

        public string register { get; set; }

        public string client { get; set; }

        /// <summary>
        /// Код юниона
        /// </summary>
        public string union { get; set; }

        public string shortname { get; set; }

        public Int64 saldoin { get; set; }

        public Int64 saldomin { get; set; }

        public Int64 bought { get; set; }

        public Int64 sold { get; set; }

        public Int64 saldo { get; set; }

        public Int64 ordbuy { get; set; }

        public Int64 ordsell { get; set; }

        /// <summary>
        /// Текущая оценка стоимости позиции, в валюте инструмента
        /// </summary>
        public Int64 amount { get; set; }

        /// <summary>
        /// Текущая оценка стоимости позиции, в рублях
        /// </summary>
        public Int64 equity { get; set; }
    }
}

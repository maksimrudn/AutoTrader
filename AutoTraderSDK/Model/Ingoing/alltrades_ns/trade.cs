using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing.alltrades_ns
{
    //сделка
    public class trade
    {
        /// <summary>
        /// Идентификатор бумаги
        /// </summary>
        public int secid { get; set; }

        /// <summary>
        /// биржевой номер сделки
        /// </summary>
        public Int64 tradeno { get; set; }

        public string board { get; set; }

        public string seccode { get; set; }


        /// <summary>
        /// время сделки
        /// </summary>
        public string time { get; set; }

      
        public double price { get; set; }

        public int quantity { get; set; }


        /// <summary>
        /// покупка (B) / продажа (S)
        /// </summary>
        public string buysell { get; set; }

        /// <summary>
        /// кол-во открытых позиций на срочном рынке
        /// </summary>
        public string openinterest { get; set; }



        /// <summary>
        /// Период торгов (O - открытие, N - торги, С - закрытие)
        /// </summary>
        public string period { get; set; }

    }
}

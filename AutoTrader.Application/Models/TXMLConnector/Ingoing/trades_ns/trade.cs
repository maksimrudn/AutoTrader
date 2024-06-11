using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing.trades_ns
{
    //сделка
    public class trade
    {

        /// <summary>
        /// Идентификатор бумаги
        /// </summary>
        public int secid { get; set; }

        /// <summary>
        /// Номер сделки на бирже
        /// </summary>
        public Int64 tradeno { get; set; }

        /// <summary>
        /// Номер заявки на бирже
        /// </summary>
        public Int64 orderno { get; set; }

        public string board { get; set; }

        public string seccode { get; set; }

        public string client { get; set; }

        /// <summary>
        /// Код юниона
        /// </summary>
        public string union { get; set; }

        public string buysell { get; set; }

        public string time { get; set; }

        /// <summary>
        /// примечание
        /// </summary>
        public string brokerref { get; set; }

        public double value { get; set; }

        public double comission { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        /// <summary>
        /// кол-во инструмента в сделках в штуках
        /// </summary>
        public Int64 items { get; set; }

        /// <summary>
        /// доходность
        /// </summary>
        public double yield { get; set; }

        /// <summary>
        /// Текущая позиция по инструменту
        /// </summary>
        public Int64 currentpos { get; set; }

        /// <summary>
        /// НКД
        /// </summary>
        public double accruedint { get; set; }

        /// <summary>
        /// тип сделки: ‘T’ – обычная ‘N’ – РПС ‘R’ – РЕПО ‘P’ – размещение
        /// </summary>
        public string tradetype { get; set; }

        /// <summary>
        /// код поставки
        /// </summary>
        public string settlecode { get; set; }



        /// <summary>
        /// Признак внебиржевой сделки, 1 или поле отсутсвует
        /// </summary>
        public string bypass { get; set; }



        /// <summary>
        /// Площадка (execution place)
        /// </summary>
        public string venue { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns
{
    public class asset
    {
        /// <summary>
        /// Код базового актива
        /// </summary>
        [XmlAttribute]
        public string code { get; set; }

        
        /// <summary>
        /// Наименование
        /// </summary>
        [XmlAttribute]
        public string name { get; set; }

        /// <summary>
        /// Ставка перекрытия
        /// </summary>
        public double setoff_rate { get; set; }

        /// <summary>
        /// Плановый риск
        /// </summary>
        public double init_req { get; set; }

        /// <summary>
        /// maint_req
        /// </summary>
        public double maint_req { get; set; }

        

        /// <summary>
        /// Наименование
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Стоимость входящей позиции
        /// </summary>
        public double open_balance { get; set; }

        /// <summary>
        /// Затрачено на покупки
        /// </summary>
        public double bought { get; set; }


        /// <summary>
        /// Выручено от продаж
        /// </summary>
        public double sold { get; set; }

        /// <summary>
        /// Стоимость текущей позиция
        /// </summary>
        public double balance { get; set; }

        /// <summary>
        /// Сумма плановых продаж
        /// </summary>
        public double estimated { get; set; }
    }
}

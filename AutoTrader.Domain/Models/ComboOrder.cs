﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Domain.Models
{
    /// <summary>
    ///Данные для выполнения комбо ордера
    /// </summary>
    public class ComboOrder: ICloneable
    {
        public ComboOrder()
        {
            TradingMode = TradingMode.Futures;
        }

        public TradingMode TradingMode { get; set; }

        public string Seccode { get; set;}

        public int Price { get; set; }

        public int SL { get; set; }

        public int TP { get; set; }

        public int Vol { get; set; }

        public bool ByMarket { get; set; }

        public OrderDirection OrderDirection { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
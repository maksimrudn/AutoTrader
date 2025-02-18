using AutoTrader.Domain.Models.Types;

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

        /// <summary>
        /// Sl в пунктах
        /// Если равно 0, то стоп ордер открыт не будет
        /// </summary>
        public int SL { get; set; }

        /// <summary>
        /// TP в пунктах
        /// Если равно 0, то стоп ордер открыт не будет
        /// </summary>
        public int TP { get; set; }

        public int Vol { get; set; }

        public bool ByMarket { get; set; }

        public OrderDirection OrderDirection { get; set; }

        public StopLoseOrderType StopLoseOrderType { get; set; } = StopLoseOrderType.ConditionalOrder;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

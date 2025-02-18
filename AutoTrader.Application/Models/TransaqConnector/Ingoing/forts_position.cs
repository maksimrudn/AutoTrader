namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    // todo: implement IGetKey interface
    public class forts_position
    {
        /// <summary>
        /// Код инструмента
        /// </summary>
        public int secid { get; set; }

        [Obsolete]
        public int market { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        public List<market> markets { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>
        public string seccode { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string client { get; set; }

        /// <summary>
        /// Код юниона 
        /// </summary>
        public string union { get; set; }

        /// <summary>
        /// Входящая позиция по инструменту
        /// </summary>
        public int startnet { get; set; }

        /// <summary>
        /// В заявках на покупку
        /// </summary>
        public int openbuys { get; set; }

        /// <summary>
        /// В заявках на продажу
        /// </summary>
        public int opensells { get; set; }

        /// <summary>
        /// Текущая позиция по инструменту
        /// </summary>
        public int totalnet { get; set; }

        /// <summary>
        /// Куплено
        /// </summary>
        public int todaybuy { get; set; }

        /// <summary>
        /// Продано
        /// </summary>
        public int todaysell { get; set; }

        /// <summary>
        /// Маржа для маржируемых опционов
        /// </summary>
        public double optmargin { get; set; }

        /// <summary>
        /// Вариационная маржа
        /// </summary>
        public double varmargin { get; set; }

        /// <summary>
        /// Опционов в заявках на 
        /// исполнение
        /// </summary>
        public Int64 expirationpos { get; set; }

        /// <summary>
        /// Объем использованого спот-лимита на 
        /// продажу
        /// </summary>
        public double usedsellspotlimit { get; set; }

        /// <summary>
        /// >текущий спот-лимит на продажу, установленный Брокером
        /// </summary>
        public double sellspotlimit { get; set; }

        /// <summary>
        /// нетто-позиция по всем инструментам данного спота
        /// </summary>
        public double netto { get; set; }

        /// <summary>
        /// коэффициент ГО для спота
        /// </summary>
        public double kgo { get; set; }

        public string GetKey()
        {
            return $"{client}-{union}-{secid}-{seccode}";
        }
    }
}

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class kind
    {
        /// <summary>
        /// идентификатор периода
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// количество секунд в периоде
        /// </summary>
        public string period { get; set; }

        /// <summary>
        /// наименование периода
        /// </summary>
        public string name { get; set; }
    }
}

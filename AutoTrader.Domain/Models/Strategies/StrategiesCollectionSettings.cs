namespace AutoTrader.Domain.Models.Strategies
{
    public class StrategiesCollectionSettings
    {
        public string NotificationFilename { get; set; }

        public string NotificationEmail { get; set; }

        public List<StrategySettings> StrategyList { get; private set; } = new List<StrategySettings>();

    }
}

using AutoTrader.Domain.Models.Types;

namespace AutoTrader.Domain.Models.Strategies
{
    public class StrategySettings
    {
        public string Seccode { get; set; }

        public int Difference { get; set; }

        public DifferenceTypes DifferenceType { get; set; }

        public SecurityPeriods Period { get; set; }

        public int Delay { get; set; }

        public NotificationTypes NotificationType { get; set; }
    }
}

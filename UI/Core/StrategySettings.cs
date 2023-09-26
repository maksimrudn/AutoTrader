using AutoTraderSDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public class StrategySettings
    {
        public string Seccode { get; set; }

        public string Difference { get; set; }

        public string Period { get; set; }

        public string Delay { get; set; }

        public NotificationTypes NotificationType { get; set; }
    }
}

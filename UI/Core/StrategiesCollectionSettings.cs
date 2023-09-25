﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public class StrategiesCollectionSettings
    {
        public string NotificationFilename { get; set; }

        public string NotificationEmail { get; set; }

        public List<StrategySettings> StrategyList { get; private set; } = new List<StrategySettings>();

    }
}

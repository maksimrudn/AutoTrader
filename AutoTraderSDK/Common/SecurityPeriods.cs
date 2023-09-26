﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Common
{
    /// <summary>
    /// В period необходимо указать нужное значение поля <id> из структуры <candlekinds>
    /// </summary>
    public enum SecurityPeriods
    {
        M1 = 1,
        M5 = 2,
        M15 = 3,
        H1 = 4,
        D1 = 5,
        W1 = 6,
    }
}

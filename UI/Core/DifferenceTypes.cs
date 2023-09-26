using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public enum DifferenceTypes
    {
        Oversold, // current price is below previous
        Overbought, // current price is above previous
        Both 
    }
}

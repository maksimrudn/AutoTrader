using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Kernel
{
    public class OnMCPositionsUpdatedEventArgs : EventArgs
    {
        public OnMCPositionsUpdatedEventArgs(mc_portfolio mc_portfolio)
        {
            data = mc_portfolio;
        }
        public mc_portfolio data { get; set; }
    }
}

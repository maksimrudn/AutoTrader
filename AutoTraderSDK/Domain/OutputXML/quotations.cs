using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.OutputXML
{
    public class quotations
    {
        public quotations()
        {
            security = new security();
        }

        public security security;
    }
}

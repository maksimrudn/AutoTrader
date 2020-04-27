using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Domain.OutputXML
{
    public class quotes
    {
        public quotes()
        {
            security = new security();

        }


        //to subscribe command
        public security security { get; set; }
    }
}

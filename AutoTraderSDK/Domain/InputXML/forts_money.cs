using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class forts_money
    {
        public string client { get; set; }

        public int market { get; set; }

        public string shortname { get; set; }

        public double current { get; set; }

        public double blocked { get; set; }

        public double free { get; set; }

        public double varmargin { get; set; }
    }
}

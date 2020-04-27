using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class money_position
    {
        public string client { get; set; }

        public int market { get; set; }

        public string register { get; set; }

        public string asset { get; set; }

        public string shortname { get; set; }

        public double saldoin { get; set; }

        public double bought { get; set; }

        public double sold { get; set; }

        public double saldo { get; set; }

        public double ordbuy { get; set; }

        public double ordbuycond { get; set; }

        public double commission { get; set; }
    }
}

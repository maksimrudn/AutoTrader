using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class forts_position
    {
        public int secid { get; set; }

        public int market { get; set; }

        public string seccode { get; set; }

        public string client { get; set; }

        public int startnet { get; set; }

        public int openbuys { get; set; }

        public int opensells { get; set; }

        public int totalnet { get; set; }

        public int todaybuy { get; set; }

        public int todaysell { get; set; }

        public double optmargin { get; set; }

        public double varmargin { get; set; }

        public Int64 expirationpos { get; set; }

        public double usedsellspotlimit { get; set; }

        public double sellspotlimit { get; set; }

        public double netto { get; set; }

        public double kgo { get; set; }
    }
}

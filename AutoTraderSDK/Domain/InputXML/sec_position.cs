using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class sec_position
    {
        public int secid { get; set; }

        public int market { get; set; }

        public string seccode { get; set; }

        public string register { get; set; }

        public string client { get; set; }

        public string shortname { get; set; }

        public Int64 saldoin { get; set; }

        public Int64 saldomin { get; set; }

        public Int64 bought { get; set; }

        public Int64 sold { get; set; }

        public Int64 saldo { get; set; }

        public Int64 ordbuy { get; set; }

        public Int64 ordsell { get; set; }
    }
}

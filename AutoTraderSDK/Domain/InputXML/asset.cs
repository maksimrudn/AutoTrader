using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class asset
    {
        public class security
        {
            public string market { get; set; }

            public string seccode { get; set; }

            public double price { get; set; }

            public double open_balance { get; set; }

            public double bought { get; set; }

            public double sold { get; set; }

            public double balance { get; set; }

            public double balance_prc { get; set; }

            public double unrealized_pnl { get; set; }

            public double buying { get; set; }

            public double selling { get; set; }

            public double equity { get; set; }

            public double reg_equity { get; set; }

            public double riskrate_long { get; set; }

            public double riskrate_short { get; set; }

            public double reserate_long { get; set; }

            public double reserate_short { get; set; }

            public double pl { get; set; }

            public double pnl_income { get; set; }

            public double pnl_intraday { get; set; }

            public double maxbuy { get; set; }

            public double maxsell { get; set; }

            public value_part value_part { get; set; }
        }

        [XmlAttribute]
        public string code { get; set; }

        [XmlAttribute]
        public string name { get; set; }


        public double setoff_rate { get; set; }

        public double init_req { get; set; }

        public double maint_req { get; set; }

        [XmlElement("security")]
        public security securityElement { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain
{
    public class portfolio_currency
    {
        [XmlAttribute]
        public string currency { get; set; }

        [XmlElement("cross_rate")]
        public double cross_rate { get; set; }

        [XmlElement("open_balance")]
        public double open_balance { get; set; }

        [XmlElement("balance")]
        public double balance { get; set; }

        [XmlElement("equity")]
        public double equity { get; set; }

        [XmlElement("cover")]
        public double cover { get; set; }

        [XmlElement("init_req")]
        public double init_req { get; set; }

        [XmlElement("maint_req")]
        public double maint_req { get; set; }

        [XmlElement("unrealized_pnl")]
        public double unrealized_pnl { get; set; }
    }
}

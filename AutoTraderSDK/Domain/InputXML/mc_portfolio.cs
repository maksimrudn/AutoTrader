using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class mc_portfolio
    {
        [XmlAttribute]
        public string union { get; set; }
        [XmlAttribute]
        public string client { get; set; }

        [XmlElement]
        public double open_equity { get; set; }

        [XmlElement]
        public double equity { get; set; }

        [Obsolete]
        [XmlElement]
        public double chrgoff_ir { get; set; }

        [XmlElement]
        public double init_req { get; set; }

        [Obsolete]
        [XmlElement]
        public double chrgoff_mr { get; set; }

        [XmlElement]
        public double maint_req { get; set; }

        [XmlElement]
        public double vm { get; set; }

        [XmlElement]
        public double finres { get; set; }

        [XmlElement]
        public double go { get; set; }

        [Obsolete]
        [XmlElement]
        public double vm_mma { get; set; }

        [XmlElement]
        public List<money> money { get; set; }

        [XmlElement("asset")]
        public List<asset> assets { get; set; }




        [XmlElement("pl")]
        public double pl { get; set; }


        [XmlElement("cover")]
        public double cover { get; set; }


        [XmlElement("unrealized_pnl")]
        public double unrealized_pnl { get; set; }

        [XmlElement("portfolio_currency")]
        public List<portfolio_currency> portfolio_currency { get; set; }

    }
}

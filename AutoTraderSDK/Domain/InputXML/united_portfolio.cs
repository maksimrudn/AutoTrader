using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class united_portfolio
    {
        [XmlAttribute]
        public string union { get; set; }
        [XmlAttribute]
        public string client { get; set; }

        [XmlElement]
        public double open_equity { get; set; }

        [XmlElement]
        public double equity { get; set; }

        [XmlElement]
        public double chrgoff_ir { get; set; }

        [XmlElement]
        public double init_req { get; set; }

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

        [XmlElement]
        public double vm_mma { get; set; }

        [XmlElement]
        public money money { get; set; }

        [XmlElement("asset")]
        public List<asset> assets { get; set; }
    }
}

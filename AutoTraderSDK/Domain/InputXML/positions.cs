using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class positions
    {
        public money_position money_position { get; set; }

        public sec_position sec_position { get; set; }

        [XmlElement("forts_position")]
        public List<forts_position> forts_position { get; set; }

        public forts_money forts_money { get; set; }

        public forts_collaterals forts_collaterals { get; set; }

        public spot_limit spot_limit { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    [Serializable, XmlRoot("sec_info_upd"), XmlType("sec_info_upd", Namespace = "")]
    public class sec_info_upd
    {
        public int secid { get; set; }

        public int market { get; set; }

        public string seccode { get; set; }

        public double minprice { get; set; }

        public double maxprice { get; set; }

        public double buy_deposit { get; set; }

        public double sell_deposit { get; set; }

        public double bgo_c { get; set; }

        public double bgo_nc { get; set; }

        public double bgo_buy { get; set; }

        public double point_cost { get; set; }
    }
}

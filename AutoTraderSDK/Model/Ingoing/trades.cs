using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    /// <summary>
    /// Сделка(и) клиента
    /// 
    /// Передается автоматически после установки соединения (для уже совершенных сделок), а также по мере появления новых сделок.
    /// </summary>
    public class trades
    {
        [XmlElement("trade")]
        public List<trades_ns.trade> trade { get; set; }
    }
}

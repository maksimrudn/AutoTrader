using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class trades
    {
        [XmlElement("trade")]
        public List<trade> trade { get; set; }
    }
}

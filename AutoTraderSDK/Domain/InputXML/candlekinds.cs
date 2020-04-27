using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class candlekinds
    {
        [XmlElement("kind")]
        public List<kind> kind { get; set; }
    }
}

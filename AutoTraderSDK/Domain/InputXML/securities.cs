using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class securities
    {
        [XmlElement("security")]
        public List<security> security { get; set; }
    }
}

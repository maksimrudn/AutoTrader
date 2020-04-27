using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class quotes
    {
        [XmlElement("quote")]
        public List<quote> quote { get; set; }
        
    }

}

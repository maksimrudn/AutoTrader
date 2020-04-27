using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML
{
    public class orders
    {
        [XmlElement("order")]
        public List<order> order { get; set; }
    }
}

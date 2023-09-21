using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.OutputXML
{
    public class quotations
    {
        public quotations()
        {
            quotation = new List<InputXML.quotations_ns.quotation>();
        }

        [XmlElement("quotation")]
        public List<InputXML.quotations_ns.quotation> quotation;
    }
}

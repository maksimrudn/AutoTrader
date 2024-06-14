using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class error
    {
        [XmlText]
        public string Text { get; set; }
    }
}

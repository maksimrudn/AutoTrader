
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Outgoing.command_ns
{
    /// <summary>
    ///  подписка на изменения показателей торгов
    /// </summary>
    public class quotations
    {
        public quotations()
        {
            security = new List<security>();
        }

        [XmlElement("security")]
        public List<security> security;
    }
}

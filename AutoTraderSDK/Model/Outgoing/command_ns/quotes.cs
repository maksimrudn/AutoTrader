using AutoTraderSDK.Model.Ingoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Outgoing.command_ns
{
    /// <summary>
    /// подписка на изменения «стакана»
    /// </summary>
    public class quotes
    {
        public quotes()
        {
            security = new List<security>();

        }


        //to subscribe command
        [XmlElement("security")]
        public List<security> security { get; set; }
    }
}

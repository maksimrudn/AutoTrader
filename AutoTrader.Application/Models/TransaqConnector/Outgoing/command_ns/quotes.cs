﻿using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Outgoing.command_ns
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

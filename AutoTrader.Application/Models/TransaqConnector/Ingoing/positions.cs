﻿using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    public class positions
    {
        public money_position money_position { get; set; }

        [XmlElement("sec_position")]
        public List<sec_position> sec_position { get; set; }

        [XmlElement("forts_position")]
        public List<forts_position> forts_position { get; set; }

        /// <summary>
        /// деньги ФОРТС
        /// </summary>
        public forts_money forts_money { get; set; }

        /// <summary>
        /// залоги ФОРТС
        /// </summary>
        public forts_collaterals forts_collaterals { get; set; }

        /// <summary>
        /// лимиты ФОРТС
        /// </summary>
        public spot_limit spot_limit { get; set; }

        public united_limits united_limits { get; set; }
    }
}

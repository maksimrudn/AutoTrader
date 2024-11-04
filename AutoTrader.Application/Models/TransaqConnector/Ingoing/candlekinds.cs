﻿using System.Xml.Serialization;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing
{
    /// <summary>
    /// Информация о доступных периодах свечей
    /// </summary>
    public class candlekinds
    {
        [XmlElement("kind")]
        public List<kind> kind { get; set; }
    }
}

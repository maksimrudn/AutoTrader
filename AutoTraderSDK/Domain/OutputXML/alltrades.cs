using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.OutputXML
{
    /// <summary>
    /// Сделки по инструменту(ам)
    /// 
    /// Передается после подписки путем команды subscribe. В сообщении могут 
    ///    быть переданы не все поля, а только те, у которых есть значения.
    ///    Информация по следкам РПС, РЕПО и сделкам по неполным лотам не
    ///    передается.
    /// Параметр open interest передается только для фьючерсов и опционов.
    /// </summary>
    public class alltrades
    {
        public alltrades()
        {
            trades = new List<InputXML.alltrades_ns.trade>();
        }

        [XmlElement("trade")]
        public List<InputXML.alltrades_ns.trade> trades;
    }
}

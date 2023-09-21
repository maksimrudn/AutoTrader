using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Domain.InputXML.orders_ns
{
    public class order
    {
        [XmlAttribute]
        public int transactionid { get; set; }

        public Int64 orderno { get; set; }

        public int secid { get; set; }

        public string board { get; set; }

        public string seccode { get; set; }

        public string client { get; set; }

        public status status { get; set; }

        public string buysell { get; set; }

        public string time { get; set; }

        public string brokerref { get; set; }

        public double value { get; set; }

        public double accruedint { get; set; }

        public string settlecode { get; set; }

        public int balance { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public string hidden { get; set; }

        public double yield { get; set; }

        public string withdrawtime { get; set; }

        public string condition { get; set; }

        public double maxcomission { get; set; }

        public string result { get; set; }
    }



    //none
    //active Активная
    //cancelled Снята трейдером (заявка уже попала на рынок и
    //была отменена)
    //denied Отклонена Брокером
    //disabled Прекращена трейдером (условная заявка,
    //которую сняли до наступления условия)
    //expired Время действия истекло
    //failed Не удалось выставить на биржу
    //forwarding Выставляется на биржу
    //inactive Статус не известен из-за проблем со связью с
    //биржей
    //matched Исполнена
    //refused Отклонена контрагентом
    //rejected Отклонена биржей
    //removed Аннулирована биржей
    //wait Не наступило время активации
    //watching Ожидает наступления условия
}

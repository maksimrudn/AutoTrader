using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Outgoing
{
    public class takeprofit
    {
        public double activationprice { get; set; }

        public double orderprice { get; set; }

        public double correction { get; set; }

        public double spread { get; set; }
                    //Защитный спрэд, объем quantity для stop loss и коррекцию можно задавать как
                    //в абсолютной величине, так и в процентах (от цены либо от позиции клиента
                    //по смыслу). Для задания процентов, достаточно поставить после числа символ
                    //'%', например:
                    //<quantity>10%</quantity>

        public int quantity { get; set; }

        public bymarket bymarket { get; set; }
    }
}

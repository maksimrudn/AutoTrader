using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Domain.OutputXML
{
    public enum cond_type
    {
        Bid,         // лучшая цена покупки // не ниже  //заявка выводится если цена выше заданной
        BidOrLast,   // лучшая цена покупки или сделка по заданной цене и выше
        Ask,         //лучшая цена продажи // ЗАЯВКА СРАБАТЫВАЕТ ЕСЛИ ЦЕНА НИЖЕ ЗАДАННОЙ
        AskOrLast,      //  = лучшая цена продажи или сделка по заданной цене и ниже     //не выше
        Time,        //время выставления заявки на Биржу
        CovDown,     //обеспеченность ниже заданной
        CovUp,      //обеспеченность выше заданной
        LastUp,      //сделка на рынке по заданной цене или выше
        LastDown,       // сделка на рынке по заданной цене или ниже
    }
}

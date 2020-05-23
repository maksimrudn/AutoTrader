using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Domain.OutputXML
{
    public enum cond_type
    {
        /// <summary>
        /// лучшая цена покупки // не ниже  //заявка выводится если цена выше заданной
        /// </summary>
        Bid,

        /// <summary>
        /// учшая цена покупки или сделка по заданной цене и выше
        /// </summary>
        BidOrLast,

        /// <summary>
        /// лучшая цена продажи // ЗАЯВКА СРАБАТЫВАЕТ ЕСЛИ ЦЕНА НИЖЕ ЗАДАННОЙ
        /// </summary>
        Ask,

        /// <summary>
        /// лучшая цена продажи или сделка по заданной цене и ниже     //не выше
        /// </summary>
        AskOrLast,

        /// <summary>
        /// время выставления заявки на Биржу
        /// </summary>
        Time,

        /// <summary>
        /// обеспеченность ниже заданной
        /// </summary>
        CovDown,

        /// <summary>
        /// обеспеченность выше заданной
        /// </summary>
        CovUp,

        /// <summary>
        /// сделка на рынке по заданной цене или выше
        /// </summary>
        LastUp,

        /// <summary>
        /// сделка на рынке по заданной цене или ниже
        /// </summary>
        LastDown
    }
}

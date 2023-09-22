using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Model.Ingoing
{
    public class kind
    {
        /// <summary>
        /// идентификатор периода
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// количество секунд в периоде
        /// </summary>
        public string period { get; set; }

        /// <summary>
        /// наименование периода
        /// </summary>
        public string name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class forts_collaterals
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int client { get; set; }

        /// <summary>
        /// Код юниона
        /// </summary>
        public string union { get; set; }

        [Obsolete]
        public int market { get; set; }

        /// <summary>
        /// Внутренний код рынка
        /// </summary>
        public List<market> markets { get; set; }

        /// <summary>
        /// Наименование вида средств
        /// </summary>
        public string shortname { get; set; }

        /// <summary>
        /// Текущие
        /// </summary>
        public double current { get; set; }

        /// <summary>
        /// Заблокировано
        /// </summary>
        public double blocked { get; set; }

        /// <summary>
        /// Свободно
        /// </summary>
        public double free { get; set; }
    }
}

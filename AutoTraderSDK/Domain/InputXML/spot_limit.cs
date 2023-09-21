using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    /// <summary>
    /// лимиты ФОРТС
    /// </summary>
    public class spot_limit
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
        /// Наименование вида средств
        /// </summary>
        public double buylimit { get; set; }

        /// <summary>
        /// Заблокировано лимита
        /// </summary>
        public double buylimitused { get; set; }
    }
}

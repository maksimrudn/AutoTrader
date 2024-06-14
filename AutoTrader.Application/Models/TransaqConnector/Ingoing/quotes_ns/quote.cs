using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTrader.Application.Models.TransaqConnector.Ingoing.quotes_ns
{
    public class quote
    {
        public int secid { get; set; }

        public string board { get; set; }

        public string seccode { get; set; }

        public double price { get; set; }

        public string source { get; set; }

        public int yield { get; set; }

        public int buy { get; set; }

        public int sell { get; set; }

            // Значение «-1» в поле buy означает, что в данной строке «стакана»
            //больше нет заявок на покупку.
            // Значение «-1» в поле sell означает, что в данной строке «стакана»
            //больше нет заявок на продажу.
            // Значение «-1» одновременно и в поле sell и в поле buy означает, что
            //строка с данной ценой (или с данным значением пары price + source)
            //удалена из «стакана».
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderSDK.Domain.InputXML
{
    public class spot_limit
    {
        public int client { get; set; }

        public int market { get; set; }

        public string shortname { get; set; }

        public double buylimit { get; set; }

        public double buylimitused { get; set; }
    }
}

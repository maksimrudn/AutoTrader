using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Model
{
    public class ClientInfoUpdatedEventArgs: EventArgs
    {
        public string ClientId { get; set; }

        public string Union { get; set; }

        public string FreeMoney { get; set; }
    }
}

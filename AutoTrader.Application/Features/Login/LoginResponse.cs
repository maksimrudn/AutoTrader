using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.Login
{
    public class LoginResponse
    {
        public List<string> SeccodeList { get; internal set; }
        public string SelectedSeccode { get; internal set; }
        public string ClientId { get; internal set; }
        public string FreeMoney { get; internal set; }
        public string Union { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderUI.Controls
{
    public class ObserversDataGridAddException : Exception
    {
        public ObserversDataGridAddException() : base("Observer for this security is already created") { }
    }
}

using System;

namespace AutoTraderUI.Controls
{
    public class ObserversDataGridAddException : Exception
    {
        public ObserversDataGridAddException() : base("Observer for this security is already created") { }
    }
}

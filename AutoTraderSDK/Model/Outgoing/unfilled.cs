using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Model.Outgoing
{
    public enum unfilled
    {
        PutInQueue,     //неисполненная часть заявки помещается в очередь заявок Биржи.
        ImmOrCancel,     //сделки совершаются только в том случае, если заявка может быть удовлетворена полностью.
        CancelBalance     //неисполненная часть заявки снимается с торгов
    }
}

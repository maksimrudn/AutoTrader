using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Domain.Models
{
    /// <summary>
    /// Тип подключение - демо/продуктив
    /// </summary>
    public enum ConnectionType
    {
        Prod,   // tr1.finam.ru:3900
        Demo    // tr1-demo5.finam.ru:3939
    }
}

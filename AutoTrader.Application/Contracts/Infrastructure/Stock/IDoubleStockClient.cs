using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface IDoubleStockClient
    {
        IStockClient Master { get; }

        IStockClient Slave { get; }
    }
}

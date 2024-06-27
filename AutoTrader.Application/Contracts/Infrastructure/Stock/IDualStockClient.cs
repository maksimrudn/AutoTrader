using AutoTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface IDualStockClient
    {
        IStockClient Master { get; }

        IStockClient Slave { get; }

        Task MakeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode);
    }
}

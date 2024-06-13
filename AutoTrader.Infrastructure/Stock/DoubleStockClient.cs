using AutoTrader.Application.Contracts.Infrastructure.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock
{
    public class DoubleStockClient: IDoubleStockClient, IAsyncDisposable
    {
        public IStockClient Master { get; }

        public IStockClient Slave { get; }

        public DoubleStockClient() {
            Master = new StockClient("txmlconnector1.dll");
            Slave = new StockClient("txmlconnector2.dll");
        }


        private bool _disposed = false;
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool dispossing)
        {
            if (!_disposed)
            {
                if (dispossing)
                {

                }

                await Master.DisposeAsync();
                await Slave.DisposeAsync();

                _disposed = true;
            }            
        }
    }
}

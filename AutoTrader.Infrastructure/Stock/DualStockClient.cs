using AutoTrader.Application.Contracts.Infrastructure.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock
{
    public class DualStockClient: IDualStockClient, IAsyncDisposable
    {
        public IStockClient Master { get; }

        public IStockClient Slave { get; }

        public DualStockClient(StockClientMaster stockClientMaster, StockClientSlave stockClientSlave) {
            Master = stockClientMaster;
            Slave = stockClientSlave;
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

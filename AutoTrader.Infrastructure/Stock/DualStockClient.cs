using AutoTrader.Application.Contracts.Infrastructure.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock
{
    public class DualStockClient: IDualStockClient, IDisposable
    {
        public IStockClient Master { get; }

        public IStockClient Slave { get; }

        public DualStockClient(StockClientMaster stockClientMaster, StockClientSlave stockClientSlave) {
            Master = stockClientMaster;
            Slave = stockClientSlave;
        }


        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispossing)
        {
            if (!_disposed)
            {
                if (dispossing)
                {

                }

                Master.Dispose();
                Slave.Dispose();
                _disposed = true;
            }            
        }
    }
}

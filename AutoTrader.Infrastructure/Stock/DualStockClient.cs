using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Helpers;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Domain.Models;
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

        public async Task MakeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode)
        {
            ComboOrder comboOrder1 = new ComboOrder();
            comboOrder1.TradingMode = TradingMode.Futures;
            comboOrder1.SL = sl;
            comboOrder1.TP = tp;
            comboOrder1.Price = price;
            comboOrder1.Vol = vol;
            comboOrder1.ByMarket = bymarket;
            comboOrder1.Seccode = seccode;
            comboOrder1.OrderDirection = OrderDirection.Buy;

            ComboOrder comboOrder2 = (ComboOrder)comboOrder1.Clone();
            comboOrder2.OrderDirection = OrderDirection.Sell;

            Task md1 = Task.Run(async () =>
            {
                await Master.CreateNewComboOrder(comboOrder1).ConfigureAwait(false);
            });

            Task md2 = Task.Run(async () =>
            {
                await Slave.CreateNewComboOrder(comboOrder2).ConfigureAwait(false);
            });

            await Task.WhenAll(md1, md2).ConfigureAwait(false);
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

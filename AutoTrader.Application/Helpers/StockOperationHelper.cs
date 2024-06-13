using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models;
using AutoTrader.Domain.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Helpers
{
    public class StockOperationHelper
    {
        public static async Task HandleComboOperation(IStockClient cl, ComboOrder co)
        {
            if (string.IsNullOrEmpty(co.Seccode))
            {
                throw new Exception("Не выбран код инструмента");
            }
            else
            {
                await cl.CreateNewComboOrder(co);
            }
        }

        public static async Task MakeMultidirect(IDoubleStockClient stockClients, int price, int vol, int sl, int tp, bool bymarket, string seccode)
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
                await StockOperationHelper.HandleComboOperation(stockClients.Master, comboOrder1);
            });

            Task md2 = Task.Run(async () =>
            {
                await StockOperationHelper.HandleComboOperation(stockClients.Slave, comboOrder2);
            });

            await Task.WhenAll(md1, md2);
        }
    }
}

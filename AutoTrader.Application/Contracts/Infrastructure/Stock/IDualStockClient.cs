namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface IDualStockClient
    {
        IStockClient Master { get; }

        IStockClient Slave { get; }

        Task MakeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode);
    }
}

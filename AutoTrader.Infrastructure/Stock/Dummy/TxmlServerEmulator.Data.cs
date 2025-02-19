using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns.money_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.orders_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.securities_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.trades_ns;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public partial class TxmlServerEmulator
    {

        

        private readonly List<client> _clients = new List<client>()
        {
            new client()
            {
                id = Constants.TestFortsClientId,
                remove = false,
                market = 4, // 4 - forts, futures
                currency = "RUB",
                type = "spot",
                union = Constants.TestUnion,
                forts_acc = Constants.TestFortsClientId
            },
            new client()
            {
                id = Constants.TestMarket1Client,
                remove = false,
                market = 1,
                currency = "RUB",
                type = "leverage",
                union = Constants.TestUnion,
            },
            new client()
            {
                id = "7GGDS/7GGDS",
                remove = false,
                market = 15,
                currency = "RUB",
                type = "leverage",
                union = Constants.TestUnion,
            },
            new client()
            {
                id = "7GGDB/7GGDB",
                remove = false,
                market = 7,
                currency = "USD",
                type = "leverage",
                union = Constants.TestUnion,
            },
        };
        
        private readonly server_status _serverStatus = new server_status()
        {
            connected = "false",
            sys_ver = 643,
            build = 3,
            server_tz = "Russian Standard Time",
            id = 1
        };

        private readonly overnight _overnight = new overnight() {status = "false"};

        private readonly positions _positions = new positions()
        {
            money_position = new money_position()
            {
                currency = "RUB",
                client = Constants.TestMarket1Client,
                union = Constants.TestUnion,
                markets = new List<market>() {new market() {id = 1,}},
                asset = "FOND_MICEX",
                shortname = "Деньги КЦБ ММВБ (RUR)",
                saldo = Constants.TestInitialSaldoBalance,
                saldoin = Constants.TestInitialSaldoBalance
            },
            forts_position =  new List<forts_position>()
        };

        private readonly mc_portfolio _mcPortfolio = new mc_portfolio()
        {
            union = Constants.TestUnion,
            client = Constants.TestMarket1Client,
            open_equity = Constants.TestInitialSaldoBalance,
            cover = Constants.TestInitialSaldoBalance,
            portfolio_currency = new List<portfolio_currency>()
            {
                new portfolio_currency()
                {
                    currency = "RUB",
                    cross_rate = 1,
                    open_balance = Constants.TestInitialSaldoBalance,
                    equity = Constants.TestInitialSaldoBalance,
                    balance = Constants.TestInitialSaldoBalance,
                    cover = Constants.TestInitialSaldoBalance
                }
            },
            moneys = new List<money>()
            {
                new money()
                {
                    name = "Фондовый рынок МБ",
                    currency = "RUB",
                    open_balance = Constants.TestInitialSaldoBalance,
                    cover = Constants.TestInitialSaldoBalance,
                    value_parts = new List<value_part>()
                    {
                        new value_part()
                        {
                            open_balance = Constants.TestInitialSaldoBalance,
                            balance = Constants.TestInitialSaldoBalance,
                        }
                    }
                }
            },
            securities = new List<security>()
        };

        private readonly orders _orders = new orders()
        {
            order = new List<order>()
        };
        
        private readonly trades _trades = new trades()
        {
            trade = new List<trade>()
        };

        private readonly stock_securities _stockSecurities = new stock_securities()
        {
            security = new List<stock_security>()
            {
                new stock_security()
                {
                    board = "FUT",
                    seccode = "SiZ5",
                    sectype = "FUT",
                    currency = "RUR",
                    currencyid = "RUB",
                    shortname = "Si-12.25",
                    market = 4,
                    minstep = 1,
                    lotsize = 1,
                    lotdivider = 1,
                    quotestype = "1",
                    point_cost = 100,
                    opmask = new opmask()
                    {
                        usecredit = "yes",
                        bymarket = "yes",
                        nosplit = "nosplit",
                        fok = "no",
                        ioc = "yes",
                        immorcancel = "no",
                        cancelbalance = "yes"
                    }
                }
            }
        };
    }
}

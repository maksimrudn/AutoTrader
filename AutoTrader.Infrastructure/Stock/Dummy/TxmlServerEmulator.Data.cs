using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns.money_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.orders_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.securities_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.trades_ns;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using System.Collections;
using System.Collections.Specialized;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public partial class TxmlServerEmulator
    {
        public const string TestUsername = "TEST";
        public const string TestPassword = "TEST";
        public const string TestFortsClientId = "712asd";
        public const string TestUnion = "432143RGMIM";
        public const string TestMarket1Client = "7GASD/7GASD";
        public const int TestInitialSaldoBalance = 30_000;
        public const int DefaultGO = 15_000;
        public const int MaxOrderVolume = 10;
        public const int LimitOrderPrice = 95000;
        

        private readonly List<client> _clients = new List<client>()
        {
            new client()
            {
                id = TestFortsClientId,
                remove = false,
                market = 4, // 4 - forts, futures
                currency = "RUB",
                type = "spot",
                union = TestUnion,
                forts_acc = TestFortsClientId
            },
            new client()
            {
                id = TestMarket1Client,
                remove = false,
                market = 1,
                currency = "RUB",
                type = "leverage",
                union = TestUnion,
            },
            new client()
            {
                id = "7GGDS/7GGDS",
                remove = false,
                market = 15,
                currency = "RUB",
                type = "leverage",
                union = TestUnion,
            },
            new client()
            {
                id = "7GGDB/7GGDB",
                remove = false,
                market = 7,
                currency = "USD",
                type = "leverage",
                union = TestUnion,
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
                client = TestMarket1Client,
                union = TestUnion,
                markets = new List<market>() {new market() {id = 1,}},
                asset = "FOND_MICEX",
                shortname = "Деньги КЦБ ММВБ (RUR)",
                saldo = TestInitialSaldoBalance,
                saldoin = TestInitialSaldoBalance
            },
            forts_position =  new List<forts_position>()
        };

        private readonly mc_portfolio _mcPortfolio = new mc_portfolio()
        {
            union = TestUnion,
            client = TestMarket1Client,
            open_equity = TestInitialSaldoBalance,
            cover = TestInitialSaldoBalance,
            portfolio_currency = new List<portfolio_currency>()
            {
                new portfolio_currency()
                {
                    currency = "RUB",
                    cross_rate = 1,
                    open_balance = TestInitialSaldoBalance,
                    equity = TestInitialSaldoBalance,
                    balance = TestInitialSaldoBalance,
                    cover = TestInitialSaldoBalance
                }
            },
            moneys = new List<money>()
            {
                new money()
                {
                    name = "Фондовый рынок МБ",
                    currency = "RUB",
                    open_balance = TestInitialSaldoBalance,
                    cover = TestInitialSaldoBalance,
                    value_parts = new List<value_part>()
                    {
                        new value_part()
                        {
                            open_balance = TestInitialSaldoBalance,
                            balance = TestInitialSaldoBalance,
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

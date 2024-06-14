using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.Dummy;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock
{
    public class StockFactory: IAsyncDisposable
    {
        private TransaqConnectorInputStreamHandler _master_inputStreamHandler;
        private ITransaqConnectorRequestHandler _master_requestHandler;
        private TransaqConnectorInputStreamHandler _slave_inputStreamHandler;
        private ITransaqConnectorRequestHandler _slave_requestHandler;

        private IStockClient _master_stockClient;
        private IStockClient _slave_stockClient;

        public StockFactory() { }        

        private (TransaqConnectorInputStreamHandler, ITransaqConnectorRequestHandler) GetHandlersForMaster(bool dummyMode = false)
        {
            _master_inputStreamHandler ??= new TransaqConnectorInputStreamHandler();
            _master_requestHandler ??= dummyMode? new DummyRequestHandler(_master_inputStreamHandler):
                                                new TransaqConnectorRequestHandler("txmlconnector1.dll", _master_inputStreamHandler);

            return (_master_inputStreamHandler,  _master_requestHandler);
        }

        private (TransaqConnectorInputStreamHandler, ITransaqConnectorRequestHandler) GetHandlersForSlave(bool dummyMode = false)
        {
            _slave_inputStreamHandler ??= new TransaqConnectorInputStreamHandler();
            _slave_requestHandler ??= dummyMode ? new DummyRequestHandler(_master_inputStreamHandler) :
                                            new TransaqConnectorRequestHandler("txmlconnector2.dll", _slave_inputStreamHandler);

            return (_slave_inputStreamHandler, _slave_requestHandler);
        }

        bool _disposed = false;
        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                _master_requestHandler?.Dispose();
                _slave_requestHandler?.Dispose();

                if (_master_stockClient != null) await _master_stockClient.DisposeAsync();
                if (_slave_stockClient != null) await _slave_stockClient.DisposeAsync();
                _disposed = true;
            }
        }

        public IStockClient GetMaster(bool dummyMode = false)
        {
            _master_stockClient ??= new StockClient(GetHandlersForMaster(dummyMode));

            return _master_stockClient;
        }

        public IStockClient GetSlave(bool dummyMode = false)
        {
            _slave_stockClient ??= new StockClient(GetHandlersForSlave(dummyMode));

            return _master_stockClient;
        }
    }
}

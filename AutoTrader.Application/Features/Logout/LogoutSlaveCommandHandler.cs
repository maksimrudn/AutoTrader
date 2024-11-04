using AutoTrader.Application.Contracts.Infrastructure.Stock;
using MediatR;

namespace AutoTrader.Application.Features.Logout
{
    public class LogoutSlaveCommandHandler : IRequestHandler<LogoutSlaveCommand>
    {
        private readonly IDualStockClient _stockClients;

        public LogoutSlaveCommandHandler(IDualStockClient stockClients)
        {
            _stockClients = stockClients;
        }

        public async Task<Unit> Handle(LogoutSlaveCommand request, CancellationToken cancellationToken)
        {
            await _stockClients.Slave.Logout().ConfigureAwait(false);

            return Unit.Value;
        }
    }
}

using AutoTrader.Application.Contracts.Infrastructure.Stock;
using MediatR;

namespace AutoTrader.Application.Features.Logout
{
    public class LogoutMasterCommandHandler : IRequestHandler<LogoutMasterCommand>
    {
        private readonly IDualStockClient _stockClients;

        public LogoutMasterCommandHandler(IDualStockClient stockClients)
        {
            _stockClients = stockClients;
        }

        public async Task<Unit> Handle(LogoutMasterCommand request, CancellationToken cancellationToken)
        {
            await _stockClients.Master.Logout().ConfigureAwait(false);

            return Unit.Value;
        }
    }
}

using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using MediatR;

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword2CommandHandler : IRequestHandler<ChangePassword1Command>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ChangePassword2CommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            _settingsService = settingsService;
            _stockClients = stockClients;
        }

        public async Task<Unit> Handle(ChangePassword1Command request, CancellationToken cancellationToken)
        {
            _stockClients.Slave.ChangePassword(request.Settings.Username2, request.Settings.Password2);
            _settingsService.UpdateSettings(request.Settings);

            return Unit.Value;
        }
    }
}

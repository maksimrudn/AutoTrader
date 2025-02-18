using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using MediatR;

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword1CommandHandler : IRequestHandler<ChangePassword1Command>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ChangePassword1CommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(ChangePassword1Command request, CancellationToken cancellationToken)
        {            
            _stockClients.Master.ChangePassword(request.Settings.GetOldPassword(), request.Settings.GetNewPassword());
            _settingsService.UpdateSettings(request.Settings);

            return Unit.Value;
        }
    }
}

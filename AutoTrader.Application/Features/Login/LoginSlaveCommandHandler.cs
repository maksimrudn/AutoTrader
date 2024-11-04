using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models.Types;
using MediatR;

namespace AutoTrader.Application.Features.Login
{
    public class LoginSlaveCommandHandler : IRequestHandler<LoginSlaveCommand, LoginResponse>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public LoginSlaveCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            _settingsService = settingsService;
            _stockClients = stockClients;
        }

        public async Task<LoginResponse> Handle(LoginSlaveCommand request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            var validator = new LoginSlaveCommandValidator();
            var validationResult = await validator.ValidateAsync(request).ConfigureAwait(false);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), request.Settings.ConnectionType);

            await _stockClients.Slave.Login(request.Settings.GetUsername(), request.Settings.GetPassword(), connType).ConfigureAwait(false);

            var resp = new LoginResponse()
            {
                SelectedSeccode = request.Settings.Seccode,
                ClientId = _stockClients.Slave.FortsClientId,
                Union = _stockClients.Slave.Union,
                FreeMoney = _stockClients.Slave.Money?.ToString("N"),
            };

            return resp;
        }
    }
}

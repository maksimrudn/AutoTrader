using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Domain.Models.Types;
using MediatR;

namespace AutoTrader.Application.Features.Login
{
    public class LoginMasterCommandHandler : IRequestHandler<LoginMasterCommand, LoginResponse>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public LoginMasterCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<LoginResponse> Handle(LoginMasterCommand request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            var validator = new LoginMasterCommandValidator();
            var validationResult = await validator.ValidateAsync(request).ConfigureAwait(false);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), request.Settings.ConnectionType);

            await _stockClients.Master.Login(request.Settings.GetUsername(), 
                                            request.Settings.GetPassword(), connType)
                                        .ConfigureAwait(false);

            var _seccodeList = (await _stockClients.Master.GetSecurities().ConfigureAwait(false))
                                        .Where(x => x.board == boardsCode.FUT.ToString())
                                        .Select(x => x.seccode)
                                        .OrderBy(x => x)
                                        .ToList();

            var resp = new LoginResponse()
            {
                SeccodeList = _seccodeList,
                SelectedSeccode = request.Settings.Seccode,
                ClientId = _stockClients.Master.FortsClientId,
                Union = _stockClients.Master.Union,
                FreeMoney = _stockClients.Master.Money?.ToString("N"),
            };        

            return resp;
        }
    }
}

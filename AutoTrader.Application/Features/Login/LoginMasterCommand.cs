using AutoTrader.Application.Models;
using MediatR;

namespace AutoTrader.Application.Features.Login
{
    public class LoginMasterCommand: IRequest<LoginResponse>
    {
        public Settings Settings { get; set; }
    }
}

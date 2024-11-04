using AutoTrader.Application.Models;
using MediatR;

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword2Command: IRequest
    {
        public Settings Settings { get; set; }
    }
}

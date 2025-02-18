using AutoTrader.Application.Models;

namespace AutoTrader.Application.Features
{
    public abstract class BaseCommand
    {
        public Settings Settings { get; set; }
    }
}

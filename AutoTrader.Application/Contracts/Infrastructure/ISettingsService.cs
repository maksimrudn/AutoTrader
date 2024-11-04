using AutoTrader.Application.Models;

namespace AutoTrader.Application.Contracts.Infrastructure
{
    public interface ISettingsService
    {
        Settings GetSettings();

        void UpdateSettings(Settings settings);
    }
}

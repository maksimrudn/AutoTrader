using AutoTrader.Application.Features.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure
{
    public interface ISettingsService
    {
        Settings GetSettings();

        void UpdateSettings(Settings settings);
    }
}

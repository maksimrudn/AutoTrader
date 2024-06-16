
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Models.Email;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoTrader.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            bool dummyMode = true;

            var s = configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton<ITransaqConnectorFactory, TransaqConnectorFactory>(sp =>
            {
                return new TransaqConnectorFactory(dummyMode);
            });
            services.AddSingleton<StockClientMaster>();
            services.AddSingleton<StockClientSlave>();
            services.AddSingleton<IDualStockClient, DualStockClient>();

            return services;
        }
    }
}

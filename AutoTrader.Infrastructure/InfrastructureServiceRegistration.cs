
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Models.Email;
using AutoTrader.Infrastructure.Stock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoTrader.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var s = configuration.GetSection("EmailSettings");

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddSingleton<StockFactory>();

            services.AddSingleton<IDualStockClient, DualStockClient>((sp) => {
                var factory = sp.GetService<StockFactory>();
                return new DualStockClient(factory.GetMaster(false), factory.GetSlave(false));
            });
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}


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

            List<IStockClient> connectors = new List<IStockClient>();
            connectors.Add(new StockClient("txmlconnector1.dll"));
            connectors.Add(new StockClient("txmlconnector2.dll"));
            //connectors.Add(new TXMLDummyConnector());
            //connectors.Add(new TXMLDummyConnector());
            services.AddSingleton<List<IStockClient>>(connectors);
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}

using AutoMapper;
using AutoTrader.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrader.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<StrategyManager>();

            return services;
        }
    }
}

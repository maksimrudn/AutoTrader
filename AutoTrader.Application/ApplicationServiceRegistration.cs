using AutoMapper;
using AutoTrader.Application.Features.Strategies;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

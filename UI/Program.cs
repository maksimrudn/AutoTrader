using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.TXMLConnector;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Features.Strategies;
using AutoTrader.Infrastructure.Stock;
using AutoTraderSDK.Core;
using AutoTraderUI;
using AutoTraderUI.Common;
using AutoTraderUI.Presenters;
using AutoTraderUI.Views;
using LightInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraderUI
{
    static class Program
    {
        public static readonly ApplicationContext Context = new ApplicationContext();

        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            var presenter = ServiceProvider.GetService<MainFormPresenter>();
            presenter.Run();
        }


        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) => {
                    var configuration = context.Configuration;


                    List<ITXMLConnector> connectors = new List<ITXMLConnector>();
                    connectors.Add(new TXMLConnector("txmlconnector1.dll"));
                    connectors.Add(new TXMLConnector("txmlconnector2.dll"));


                    //connectors.Add(new TXMLDummyConnector());
                    //connectors.Add(new TXMLDummyConnector());


                    services.AddSingleton<List<ITXMLConnector>>(connectors);
                    services.AddSingleton<ApplicationContext>(Context);

                    services.AddSingleton<IAppSettingsService, SettingsService>();
                    services.AddTransient<IEmailService, EmailService>();
                    services.AddTransient<StrategyManager>();

                    services.AddSingleton<MainForm>();
                    services.AddSingleton<MainFormPresenter>();
                    services.AddSingleton<CreateEditObserver>();
                });
        }
    }
}

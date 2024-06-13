using AutoTrader.Application;
using AutoTrader.Application.Contracts.UI;
using AutoTrader.Infrastructure;
using AutoTraderUI.Presenters;
using AutoTraderUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            try
            {
                var presenter = ServiceProvider.GetService<MainFormPresenter>();
                presenter.Run();
            }
            finally
            {
                await DisposeServices();
            }
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

                    services.AddInfrastructureServices(context.Configuration);
                    services.AddApplicationServices();                    

                    services.AddSingleton<ApplicationContext>(Context);

                    services.AddSingleton<IMainFormView, MainForm>();
                    services.AddSingleton<MainForm>(serviceProvider =>
                    {
                        var mainForm = (MainForm)serviceProvider.GetRequiredService<IMainFormView>();
                        return mainForm;
                    });

                    services.AddSingleton<MainFormPresenter>();
                    services.AddSingleton<CreateEditObserver>();
                });
        }
        
        private static async Task DisposeServices()
        {
            if (ServiceProvider is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
        }
    }
}

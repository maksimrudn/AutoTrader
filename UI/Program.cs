using AutoTrader.Application;
using AutoTrader.Application.Contracts.UI;
using AutoTrader.Infrastructure;
using AutoTraderUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraderUI
{
    static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += GlobalExceptionHandler;

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            try
            {
                var mainForm = ServiceProvider.GetService<MainForm>();
                Application.Run(mainForm);
            }
            finally
            {
                await DisposeServices();
            }
        }
        
        private static void GlobalExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show($"An error occurred: {e.Exception.Message}");
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

                    services.AddInfrastructureServices(context.Configuration)
                            .AddApplicationServices();

                    services.AddSingleton<IMainFormView, MainForm>();
                    services.AddSingleton<MainForm>(serviceProvider =>
                    {
                        var mainForm = (MainForm)serviceProvider.GetRequiredService<IMainFormView>();
                        return mainForm;
                    });

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

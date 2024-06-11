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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<ITXMLConnector> connectors = new List<ITXMLConnector>();
            connectors.Add(new TXMLConnector("txmlconnector1.dll"));
            connectors.Add(new TXMLConnector("txmlconnector2.dll"));


            //connectors.Add(new TXMLDummyConnector());
            //connectors.Add(new TXMLDummyConnector());

            ServiceContainer container = new ServiceContainer();
            container.RegisterInstance<List<ITXMLConnector>>(connectors);
            container.RegisterInstance<AppSettings>(AutoTrader.Infrastructure.Globals.Settings);
            container.RegisterInstance<ApplicationContext>(Context);

            container.Register<IEmailService, EmailService>();
            container.Register<StrategyManager>();

            container.Register<MainForm>();
            container.Register<MainFormPresenter>();
            container.Register<CreateEditObserver>();
            

            ApplicationController controller = new ApplicationController(container);
            controller.Run<MainFormPresenter>();


        }
    }
}

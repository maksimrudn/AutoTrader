using AutoTraderSDK.Core;
using AutoTraderUI;
using AutoTraderUI.Common;
using AutoTraderUI.Core;
using AutoTraderUI.Presenters;
using AutoTraderUI.Views;
using LightInject;
using System;
using System.Collections.Generic;
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
            //connectors.Add(new TXMLConnector("txmlconnector1.dll"));
            //connectors.Add(new TXMLConnector("txmlconnector2.dll"));

            connectors.Add(new TXMLDummyConnector());
            connectors.Add(new TXMLDummyConnector());

            ServiceContainer container = new ServiceContainer();
            container.RegisterInstance<List<ITXMLConnector>>(connectors);
            container.RegisterInstance<Settings>(Globals.Settings);
            container.RegisterInstance<ApplicationContext>(Context);
            container.Register<MainForm>();
            container.Register<MainFormPresenter>();
            container.Register<CreateEditObserver>();
            container.Register<StrategiesCollection>();

            ApplicationController controller = new ApplicationController(container);
            controller.Run<MainFormPresenter>();


        }
    }
}

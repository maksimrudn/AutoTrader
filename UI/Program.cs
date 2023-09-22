using AutoTraderSDK.Core;
using AutoTraderUI;
using AutoTraderUI.Common;
using AutoTraderUI.Presenters;
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

            List<TXMLConnector> connectors = new List<TXMLConnector>();
            connectors.Add(new TXMLConnector("txmlconnector1.dll"));
            connectors.Add(new TXMLConnector("txmlconnector2.dll"));

            ServiceContainer container = new ServiceContainer();
            container.RegisterInstance<List<TXMLConnector>>(connectors);
            container.RegisterInstance<Settings>(Globals.Settings);
            container.RegisterInstance<ApplicationContext>(Context);
            container.Register<IMainFormView, MainForm>();
            container.Register<MainFormPresenter>();

            ApplicationController controller = new ApplicationController(container);
            controller.Run<MainFormPresenter>();


        }
    }
}

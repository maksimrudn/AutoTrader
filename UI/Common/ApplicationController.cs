using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Common
{
    public class ApplicationController
    {
        ServiceContainer _container;

        public ApplicationController(ServiceContainer serviceContainer)
        {
            _container = serviceContainer;
            _container.RegisterInstance<ApplicationController>(this);
        }

        public void Run<TPresenter>() where TPresenter : class, IPresenter
        {
            var presenter = _container.GetInstance<TPresenter>();
            presenter.Run();
        }
    }
}

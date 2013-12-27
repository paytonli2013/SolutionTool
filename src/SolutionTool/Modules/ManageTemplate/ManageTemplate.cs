using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManageTemplate
{
    public class ManageTemplateModule : IModule
    {
        IUnityContainer _container;

        IRegionManager _regionManager;

        public ManageTemplateModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;

            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("LeftMenuRegion", typeof(ManageTemplateMenuItemView));

            _container.RegisterType<object,ManageTemplateView>("ManageTemplate");
        }
    }
}

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Options
{
    public class OptionsModule : IModule
    {
        IUnityContainer _container;

        IRegionManager _regionManager;

        public OptionsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;

            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //throw new NotImplementedException();

            //_regionManager.RegisterViewWithRegion("ContentRegion", typeof(ManageRuleView));

            _regionManager.RegisterViewWithRegion("LeftMenuRegion", typeof(OptionsMenuItemView));

            _container.RegisterType<object, OptionsView>("Options");
        }
    }
}

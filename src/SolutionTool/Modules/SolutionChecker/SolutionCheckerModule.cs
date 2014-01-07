using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolutionChecker
{
    public class SolutionCheckerModule : IModule
    {
        IUnityContainer _container;

        IRegionManager _regionManager;

        public SolutionCheckerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;

            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("LeftMenuRegion", typeof(SolutionCheckerMenuItemView));

            _container.RegisterType<object, SolutionCheckerView>("SolutionChecker");
            _container.RegisterType<object, NewProjectView>("NewProjectView");
        }
    }
}

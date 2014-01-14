using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace ManageRule
{
    public class ManageRuleModule : IModule
    {
        IUnityContainer _container;

        IRegionManager _regionManager;

        public ManageRuleModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;

            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //throw new NotImplementedException();

            //_regionManager.RegisterViewWithRegion("ContentRegion", typeof(ManageRuleView));

            _regionManager.RegisterViewWithRegion("LeftMenuRegion", typeof(ManageRuleMenuItemView));

            _container.RegisterType<object,ManageRuleView>("ManageRule");
        }
    }
}

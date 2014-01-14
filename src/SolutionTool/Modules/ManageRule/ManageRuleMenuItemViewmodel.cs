using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;

namespace ManageRule
{
    public class ManageRuleMenuItemViewmodel : MenuItemViewmodelBase
    {
        public ManageRuleMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager, shellService)
        {
            ViewName = "ManageRule";
        }
    }
}

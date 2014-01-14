using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;

namespace ManageTemplate
{
    public class ManageTemplateMenuItemViewmodel : MenuItemViewmodelBase
    {
        public ManageTemplateMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager, shellService)
        {
            ViewName = "ManageTemplate";
        }
    }
}

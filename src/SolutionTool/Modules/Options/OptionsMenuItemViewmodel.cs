using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;

namespace Options
{
    public class OptionsMenuItemViewmodel : MenuItemViewmodelBase
    {
        public OptionsMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager,shellService)
        {
            ViewName = "Options";
        }
    }
}

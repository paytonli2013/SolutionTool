using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;

namespace SolutionChecker
{
    public class SolutionCheckerMenuItemViewmodel : MenuItemViewmodelBase
    {
        public SolutionCheckerMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager,shellService)
        {
            ViewName = "SolutionChecker";
        }
    }
}

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

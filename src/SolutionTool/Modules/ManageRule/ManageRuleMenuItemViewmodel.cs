﻿using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

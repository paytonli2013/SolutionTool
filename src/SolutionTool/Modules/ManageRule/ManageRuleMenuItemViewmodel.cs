using Microsoft.Practices.Prism.Commands;
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
        public DelegateCommand NavigateToManageRule { get { return new DelegateCommand(ExecuteNavigateToManageRule, CanExecuteNavigateToManageRule); } }

        public ManageRuleMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager,shellService)
        {

        }

        public override void OnNavigatedFrom(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext)
        {
            //base.OnNavigatedFrom(navigationContext);
            Navigate("ContentRegion", "ManageRule", (isSuccess, error) =>
            {
                if (!isSuccess && error != null)
                {
                    ShowMessage(error);
                }
            });
        }


        private bool CanExecuteNavigateToManageRule()
        {
            return true;
        }

        private void ExecuteNavigateToManageRule()
        {
            Navigate("ContentRegion", "\\ManageRule", (isSuccess, error) =>
            {
                if (!isSuccess && error != null)
                {
                    ShowMessage(error);
                }
            });
            //throw new NotImplementedException();
        }
    }
}

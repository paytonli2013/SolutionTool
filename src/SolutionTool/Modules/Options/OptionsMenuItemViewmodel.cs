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
        public DelegateCommand NavigateToOptions { get { return new DelegateCommand(ExecuteNavigateToOptions, CanExecuteNavigateToOptions); } }

        public OptionsMenuItemViewmodel(IRegionManager regionManager, IShellService shellService)
            : base(regionManager,shellService)
        {
            ViewName = "Options";
        }

        public override void OnNavigatedFrom(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext)
        {
            //base.OnNavigatedFrom(navigationContext);
            Navigate("ContentRegion", "Options", (isSuccess, error) =>
            {
                if (!isSuccess && error != null)
                {
                    ShowMessage(error);
                }
            });
        }


        private bool CanExecuteNavigateToOptions()
        {
            return true;
        }

        private void ExecuteNavigateToOptions()
        {
            Navigate("ContentRegion", "\\Options", (isSuccess, error) =>
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

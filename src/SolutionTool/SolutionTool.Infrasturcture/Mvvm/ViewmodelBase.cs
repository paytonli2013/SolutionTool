using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Mvvm
{
    public abstract class ViewmodelBase : NotificationObject, IViewModel, IConfirmNavigationRequest
    {
        protected IShellService _shellService;

        object _view;
        public object View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                RaisePropertyChanged("View");
            }
        }

        string _viewName;
        string ViewName
        {
            get
            {
                return _viewName;
            }
            set
            {
                _viewName = value;
                RaisePropertyChanged("ViewName");
            }
        }

        public ViewmodelBase(IShellService shellService)
        {
            _shellService = shellService;
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return navigationContext.Uri.ToString().Contains(ViewName);
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        protected void ShowMessage(Exception error)
        {
            ShowMessage(error.Message);
            //throw new NotImplementedException();
        }

        protected void ShowMessage(string msg)
        {
            //throw new NotImplementedException();
        }
    }
}

using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

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

        bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        protected static void RunCodeInUiThread(Action action, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher == null && Application.Current != null)
                dispatcher = Application.Current.Dispatcher;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority);
            }
            else
            {
                action.Invoke();
            }
        }

        protected static void RunCodeInUiThread<T>(Action<T> action, T parameter, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority, parameter);
            }
            else
            {
                action.Invoke(parameter);
            }
        }

        protected static void RunCodeInUiThread<T1, T2>(Action<T1, T2> action, T1 p1, T2 p2, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority, p1,p2);
            }
            else
            {
                action.Invoke(p1, p2);
            }
        }
    }
}

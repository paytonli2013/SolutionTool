using Microsoft.Practices.Prism.Regions;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Toolkit;

namespace Orc.SolutionTool
{
    public class ShellViewmodel : NotificationObject, IViewModel, IMessageService
    {
        IRegionManager _regionManager;
        public ShellViewmodel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        StatusCatgory _statusCatgory;

        public StatusCatgory StatusCatgory
        {
            get { return _statusCatgory; }
            private set { _statusCatgory = value; }
        }

        string statusMessage;

        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            private set
            {
                statusMessage = value; 
                RaisePropertyChanged("StatusMessage");
            }
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            StatusMessage = message;
        }

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

        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        internal void Avtive(int p)
        {
            var region = _regionManager.Regions["LeftMenuRegion"];
            var view = region.Views.FirstOrDefault();

            if (view != null)
            {
                region.Activate(view);
            }
            //throw new NotImplementedException();
        }

        public void Confirm(string message, Action<MessageBoxResult> onConfirmed)
        {
            //throw new NotImplementedException();
        }
    }
}

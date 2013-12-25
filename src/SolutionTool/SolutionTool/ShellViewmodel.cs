using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class ShellViewmodel : NotificationObject, IViewModel, IMessageService
    {
        public ShellViewmodel()
        {

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
            throw new NotImplementedException();
        }
    }
}

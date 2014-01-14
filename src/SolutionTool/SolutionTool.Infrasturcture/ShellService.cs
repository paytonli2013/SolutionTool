using System;
using Microsoft.Practices.Unity;

namespace Orc.SolutionTool
{
    public class ShellService : IShellService
    {
        IMessageService _messageService;
        IWindowHost _window;
        IUnityContainer _container;
        IChildViewService _childViewService;

        public ShellService(IWindowHost window,IMessageService messageService,IUnityContainer container,IChildViewService childViewService)
        {
            _messageService = messageService;
            _window = window;
            _container = container;
            _childViewService = childViewService;
        }

        public IMessageService MessageService
        {
            get { return _messageService; }
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            _window.PostStatusMessage(catgory,message);
        }

        public IWindowHost Host
        {
            get { return _window; }
        }

        public void OpenChildView(string viewName, string title, Action<CloseResult> onClosed,ViewOptions option = null)
        {
            if (_childViewService != null)
            {
                _childViewService.OpenChildView(_window, viewName, title, onClosed, option);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class ShellService : IShellService
    {
        IMessageService _messageService;
        IHostWindow _window;

        public ShellService(IMessageService messageService, IHostWindow window)
        {
            _messageService = messageService;
            _window = window;
        }

        public IMessageService MessageService
        {
            get { return _messageService; }
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            _window.PostStatusMessage(catgory,message);
        }
    }
}

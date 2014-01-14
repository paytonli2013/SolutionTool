using System;

namespace Orc.SolutionTool
{
    public interface IShellService
    {
        IWindowHost Host { get; }

        IMessageService MessageService { get; }

        void PostStatusMessage(StatusCatgory catgory, string message);

        void OpenChildView(string viewName, string title, Action<CloseResult> onClosed = null, ViewOptions option = null);
    }
}

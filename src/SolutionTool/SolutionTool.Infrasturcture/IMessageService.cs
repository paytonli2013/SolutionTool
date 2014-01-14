using System;

namespace Orc.SolutionTool
{
    public interface IMessageService
    {
        void Show(string message);

        void Confirm(string message,Action<MessageBoxResult> onConfirmed);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IMessageService
    {
        void Show(string message);

        void Confirm(string message,Action<MessageBoxResult> onConfirmed);
    }
}

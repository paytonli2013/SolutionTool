using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IShellService
    {
        IMessageService MessageService { get; }

        void PostStatusMessage(StatusCatgory catgory, string message);
    }
}

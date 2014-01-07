using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IWindowHost
    {
        string Name { get; }

        void PostStatusMessage(StatusCatgory catgory, string message);

        void Refresh();

        void Close(CloseResult result);

        object GetRealWindow();
    }
    
    [Flags]
    public enum CloseResult
    {
        Ok,
        Cancel,
        Yes,
        No,
        Success,
        Fail,
        RefreshParent
    }
}

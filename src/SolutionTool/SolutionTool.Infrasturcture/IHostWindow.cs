using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IHostWindow
    {
        void PostStatusMessage(StatusCatgory catgory, string message);
    }
}

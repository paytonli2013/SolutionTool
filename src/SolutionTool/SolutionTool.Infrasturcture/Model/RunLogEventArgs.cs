using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class RunLogEventArgs : EventArgs
    {
        RunLogItem _item;

        public RunLogItem Item
        {
            get { return _item; }
        }

        public RunLogEventArgs(RunLogItem item)
        {
            _item = item;
        }
    }
}

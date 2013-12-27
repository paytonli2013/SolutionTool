using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class ExamContext : Dictionary<string, object>
    {
        IRule _parent;

        public IRule ParentRule
        {
            get { return _parent; }
            set { _parent = value; }
        }
    }
}

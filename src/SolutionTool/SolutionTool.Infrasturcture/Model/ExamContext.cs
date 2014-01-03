using System.Collections.Generic;

namespace Orc.SolutionTool.Model
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

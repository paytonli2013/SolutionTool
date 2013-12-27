using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public abstract class RuleBase : IRule
    {
        public RuleBase()
        {
 
        }

        public abstract void Exam(ExamContext context, Action<ExamResult> onComplete);

        public abstract void Apply(ExamContext context, Action<ExamResult> onComplete);
    }
}

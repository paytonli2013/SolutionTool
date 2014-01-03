using System;

namespace Orc.SolutionTool.Model
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

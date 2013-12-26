using System;

namespace Orc.SolutionTool.Core.Rules
{
    public abstract class Rule : NotificationObject, IRule
    {
        public string Name { get { return this.GetType().Name; } }

        public abstract void Exam(Context context, Action<ExamResult> action);

        public abstract void Apply(Context context, Action<ApplyResult> action);
    }
}

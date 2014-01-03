using System;
using Orc.SolutionTool.Mvvm;

namespace Orc.SolutionTool.Model
{
    public abstract class Rule : NotificationObject
    {
        public string Name { get { return this.GetType().Name; } }

        public abstract void Exam(Context context, Action<ExamResult> action);

        public abstract void Apply(Context context, Action<ApplyResult> action);
    }
}

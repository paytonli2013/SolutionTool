using System;

namespace Orc.SolutionTool.Core
{
    public interface IRule
    {
        string Name { get; }

        void Exam(Context context, Action<ExamResult> action);

        void Apply(Context context, Action<ApplyResult> action);
    }
}

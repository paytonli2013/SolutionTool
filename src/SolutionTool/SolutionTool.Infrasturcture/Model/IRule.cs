using System;

namespace Orc.SolutionTool.Model
{
    public interface IRule
    {
        void Exam(ExamContext context, Action<ExamResult> onComplete);

        void Apply(ExamContext context, Action<ExamResult> onComplete);

        bool IsEnabled { get; set; }

        string Name { get; }

        string Description { get; }
    }
}

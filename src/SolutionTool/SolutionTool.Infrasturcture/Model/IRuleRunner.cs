using System;
using Orc.SolutionTool.Model;
namespace Orc.SolutionTool
{
    public interface IRuleRunner
    {
        void Exam(Orc.SolutionTool.ExamContext context, System.Collections.Generic.IEnumerable<Orc.SolutionTool.IRule> rules, Action<Orc.SolutionTool.ExamResult> onComplete);
        void LoadRunLog(Action<System.Collections.Generic.IEnumerable<RunLogItem>, Exception> onComplete);

        event EventHandler<RunLogEventArgs> RunLogAdded;
    }
}

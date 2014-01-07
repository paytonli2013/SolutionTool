using System;
using System.Collections.Generic;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public interface IRuleRunner
    {
        void Exam(Project project, Action<ExamResult> onComplete);

        void LoadRunLog(Action<IEnumerable<RunLogItem>, Exception> onComplete);

        void ClearLog(Action<Exception> onComplete);

        event EventHandler<RunLogEventArgs> RunLogAdded;

        void RunProject(Project project, Action<Report> onComplete);
    }
}

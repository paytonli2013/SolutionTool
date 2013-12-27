using Orc.SolutionTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class RuleRunner : IRuleRunner
    {
        public void LoadRunLog(Action<IEnumerable<RunLogItem>, Exception> onComplete)
        {
            //mock up some data
            List<RunLogItem> list = new List<RunLogItem>();

            list.Add(new RunLogItem 
            {
                Project = "Demo",
                Start = DateTime.Now.AddMinutes(-1),
                End = DateTime.Now,
                Result = ExamResult.Result.Passed,
                Summary = "Demo (4 Rules) Passed <1s"
            });
            list.Add(new RunLogItem
            {
                Project = "Demo",
                Start = DateTime.Now.AddMinutes(-1),
                End = DateTime.Now,
                Result = ExamResult.Result.Fail,
                Summary = "Demo (4 Rules) Failed <1s"
            });
            list.Add(new RunLogItem
            {
                Project = "Demo",
                Start = DateTime.Now.AddMinutes(-1),
                End = DateTime.Now,
                Result = ExamResult.Result.Fail,
                Summary = "Demo (4 Rules) Failed <3s"
            });
            if (onComplete != null)
                onComplete.Invoke(list, null);
        }

        public event EventHandler<RunLogEventArgs> RunLogAdded;

        public void Exam(ExamContext context, IEnumerable<IRule> rules, Action<ExamResult> onComplete)
        {

        }

        void FireRunLogAdded(RunLogItem item)
        {
            if (RunLogAdded != null)
            {
                RunLogAdded.Invoke(this, new RunLogEventArgs(item));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class Report
    {
        public Report(ExamContext context,RunLogItem log,Exception error=null)
        {
            // TODO: Complete member initialization
            _log = log;
            _error = error;
            this.context = context;
        }

        Exception _error;

        public Exception Error
        {
            get { return _error; }
        }

        RunLogItem _log;
        private ExamContext context;

        public RunLogItem Log
        {
            get { return _log; }
        }

        public string GetText()
        {
            var sb = new StringBuilder();
            foreach (var strs in context.Outputs)
            {
                foreach (var str in strs.Value)
                {
                    sb.AppendLine((str));
                }
            }
            return sb.ToString();
        }

        public ActionStatus Status
        {
            get
            {
                if (context == null || context.Results == null)
                    return ActionStatus.None;

                if (context.Results.Any(r => r.Status == ActionStatus.Failed))
                    return ActionStatus.Failed;
                else
                    return ActionStatus.Pass;
            }
        }
    }
}

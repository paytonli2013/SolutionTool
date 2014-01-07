using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class Report
    {
        public Report(ReportResult result,RunLogItem log,Exception error=null)
        {
            _result = result;
            _log = log;
            _error = error;
        }

        ReportResult _result;

        public ReportResult Result
        {
            get { return _result; }
        }

        Exception _error;

        public Exception Error
        {
            get { return _error; }
        }

        RunLogItem _log;

        public RunLogItem Log
        {
            get { return _log; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class ExamResult
    {
        string _summary;

        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        public enum Result
        {
            Passed,
            Fail
        }
    }
}

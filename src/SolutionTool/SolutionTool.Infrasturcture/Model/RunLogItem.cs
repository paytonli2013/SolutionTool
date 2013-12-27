using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class RunLogItem
    {
        public string Project { get; set; }

        public int Rules { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ExamResult.Result Result { get; set; }
        public string Summary { get; set; }

        public string Report { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IRule
    {
        void Exam(ExamContext context, Action<ExamResult> onComplete);

        void Apply(ExamContext context, Action<ExamResult> onComplete);
    }
}

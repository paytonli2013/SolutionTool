using System;

namespace Orc.SolutionTool.Model
{
    public class DirectoryExistedRule : FileStructureRule
    {
        public DirectoryExistedRule()
        {

        }

        protected override void InnerExam(FileStructureRuleExamContext context, Action<ExamResult> onComplete)
        {
            var exists = System.IO.Directory.Exists(context.CurrentDirctory);

            if (onComplete != null)
            {
                onComplete(new ExamResult { });
            }
        }

        protected override void InnerApply(FileStructureRuleExamContext context, Action<ExamResult> onComplete)
        {
            var exists = System.IO.Directory.Exists(context.CurrentDirctory);

            if (onComplete != null)
            {
                onComplete(new ExamResult { });
            }
        }
    }
}

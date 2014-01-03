using System;
using System.IO;

namespace Orc.SolutionTool.Model.Rules
{
    public class FolderMustExistsRule : Rule
    {
        public override void Exam(Context context, Action<ExamResult> action)
        {
            var path = Path.Combine(context.Repository.Path, context.Target.Path);
            var exists = System.IO.Directory.Exists(path);
            var result = new ExamResult();

            if (!exists)
            {
                result.Status = ActionStatus.Failed;
                result.Summary = "Folder Not Exists: " + context.Target.Path;
            }
            else
            {
                result.Status = ActionStatus.Pass;
                result.Summary = string.Empty;
            }

            if (action != null)
            {
                action(result);
            }
        }

        public override void Apply(Context context, Action<ApplyResult> action)
        {
            var path = Path.Combine(context.Repository.Path, context.Target.Path);
            var exists = System.IO.Directory.Exists(path);
            var result = new ApplyResult();

            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(context.Target.Path);

                    result.Status = ActionStatus.Pass;
                    result.Summary = "Folder is Created: " + context.Target.Path;
                }
                catch (Exception xe)
                {
                    result.Status = ActionStatus.Failed;
                    result.Summary = xe.Message;
                }
            }
            else
            {
                result.Status = ActionStatus.Pass;
                result.Summary = string.Empty;
            }

            if (action != null)
            {
                action(result);
            }
        }
    }
}

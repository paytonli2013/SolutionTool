using System;
using System.IO;

namespace Orc.SolutionTool.Core.Rules
{
    public class FolderMustExistsRule : Rule
    {
        public override void Exam(Context context, Action<ExamResult> action)
        {
            var path = Path.Combine(context.Repository.Path, context.Target.Path);
            var exists = Directory.Exists(path);
            var result = new ExamResult();

            if (!exists)
            {
                result.Status = ActionStatus.Failed;
                result.Message = "Folder Not Exists: " + context.Target.Path;
            }
            else
            {
                result.Status = ActionStatus.Pass;
                result.Message = string.Empty;
            }

            if (action != null)
            {
                action(result);
            }
        }

        public override void Apply(Context context, Action<ApplyResult> action)
        {
            var path = Path.Combine(context.Repository.Path, context.Target.Path);
            var exists = Directory.Exists(path);
            var result = new ApplyResult();

            if (!exists)
            {
                try
                {
                    Directory.CreateDirectory(context.Target.Path);

                    result.Status = ActionStatus.Pass;
                    result.Message = "Folder is Created: " + context.Target.Path;
                }
                catch (Exception xe)
                {
                    result.Status = ActionStatus.Failed;
                    result.Message = xe.Message;
                }
            }
            else
            {
                result.Status = ActionStatus.Pass;
                result.Message = string.Empty;
            }

            if (action != null)
            {
                action(result);
            }
        }
    }
}

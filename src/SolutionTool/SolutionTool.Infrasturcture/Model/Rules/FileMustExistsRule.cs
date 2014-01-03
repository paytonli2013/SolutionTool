﻿using System;
using System.IO;

namespace Orc.SolutionTool.Model.Rules
{
    public class FileMustExistsRule : Rule
    {
        private string _fileContent;
        public string FileContent
        {
            get
            {
                return _fileContent;
            }
            set
            {
                if (_fileContent != value)
                {
                    _fileContent = value;
                    RaisePropertyChanged(() => FileContent);
                }
            }
        }

        public override void Exam(Context context, Action<ExamResult> action)
        {
            var path = Path.Combine(context.Repository.Path, context.Target.Path);
            var exists = System.IO.File.Exists(path);
            var result = new ExamResult();

            if (!exists)
            {
                result.Status = ActionStatus.Failed;
                result.Summary = "File Not Exists: " + context.Target.Path;
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
            var exists = System.IO.File.Exists(path);
            var result = new ApplyResult();

            if (!exists)
            {
                try
                {
                    using (var fs = System.IO.File.CreateText(context.Target.Path))
                    {
                        fs.Write(FileContent);
                    }

                    result.Status = ActionStatus.Pass;
                    result.Summary = "File is Created: " + context.Target.Path;
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
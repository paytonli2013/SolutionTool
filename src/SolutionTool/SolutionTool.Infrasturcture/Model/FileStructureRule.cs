using System;
using System.Collections.Generic;

namespace Orc.SolutionTool.Model
{
    public abstract class FileStructureRule : RuleBase
    {
        IEnumerable<FileStructureRule> _children;

        public IEnumerable<FileStructureRule> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public FileStructureRule(IEnumerable<FileStructureRule> children) : this()
        {
            _children = children;
        }

        public FileStructureRule() : base()
        { 

        }

        public override void Exam(ExamContext context, Action<ExamResult> onComplete)
        {
            this.InnerExam(new FileStructureRuleExamContext(context, MakeCurrentDirectory()), onComplete);
        }

        private string MakeCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        protected abstract void InnerExam(FileStructureRuleExamContext context, Action<ExamResult> onComplete);

        public override void Apply(ExamContext context, Action<ExamResult> onComplete)
        {
            InnerApply(new FileStructureRuleExamContext(context, MakeCurrentDirectory()), onComplete);
        }

        protected abstract void InnerApply(FileStructureRuleExamContext fileStructureRuleExamContext, Action<ExamResult> onComplete);
    }
}

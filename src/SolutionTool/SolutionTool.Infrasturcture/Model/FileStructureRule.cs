using System;
using System.Collections.Generic;

namespace Orc.SolutionTool.Model
{
    public class FileStructureRule : RuleBase
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
            Name = "File Structure Rule";
            Description = "Description File Structure Rule";
        }

        public override void Exam(ExamContext context, Action<ExamResult> onComplete)
        {
            this.InnerExam(new FileStructureRuleExamContext(context, MakeCurrentDirectory()), onComplete);
        }

        private string MakeCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        protected virtual void InnerExam(FileStructureRuleExamContext context, Action<ExamResult> onComplete) 
        {

        }

        public override void Apply(ExamContext context, Action<ExamResult> onComplete)
        {
            InnerApply(new FileStructureRuleExamContext(context, MakeCurrentDirectory()), onComplete);
        }

        protected virtual void InnerApply(FileStructureRuleExamContext fileStructureRuleExamContext, Action<ExamResult> onComplete) 
        {

        }
    }
}

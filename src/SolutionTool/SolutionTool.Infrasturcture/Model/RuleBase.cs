using Orc.SolutionTool.Mvvm;
using System;

namespace Orc.SolutionTool.Model
{
    public abstract class RuleBase : NotificationObject, IRule
    {
        public RuleBase()
        {

        }

        public abstract void Exam(ExamContext context, Action<ExamResult> onComplete);

        public abstract void Apply(ExamContext context, Action<ExamResult> onComplete);

        bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        string _name;
        public string Name
        {
            get { return _name; }

            protected set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        string _desc;
        public string Description
        {
            get { return _desc; }

            protected set
            {
                _desc = value;
                RaisePropertyChanged("Description");
            }
        }
    }
}

using Orc.SolutionTool.Mvvm;
namespace Orc.SolutionTool.Model
{
    public enum ActionStatus
    {
        None,
        Pass,
        Failed,
    }

    public class Result : NotificationObject
    {
        private string _ruleName;
        public string RuleName
        {
            get
            {
                return _ruleName;
            }
            set
            {
                if (_ruleName != value)
                {
                    _ruleName = value;
                    RaisePropertyChanged(() => RuleName);
                }
            }
        }

        private ActionStatus _status;
        public ActionStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged(() => Status);
                }
            }
        }

        public override string ToString()
        {
            var s = string.Format("[{0}] - [{1}]", RuleName, Status);

            return s;
        }
    }

    public class ExamResult : Result
    {

    }

    public class ApplyResult : Result
    {

    }
}

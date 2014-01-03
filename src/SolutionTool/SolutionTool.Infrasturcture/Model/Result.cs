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

        private string _summary;
        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    RaisePropertyChanged(() => Summary);
                }
            }
        }

        public override string ToString()
        {
            var s = string.Format("[{0}] - [{1}] - [{2}]", this.GetType().Name, Status, Summary);

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

namespace Orc.SolutionTool.Core
{
    public enum ActionStatus
    {
        None,
        Pass,
        Failed,
    }

    public abstract class Result : NotificationObject
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

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }

        public override string ToString()
        {
            var s = string.Format("[{0}] - [{1}] - [{2}]", this.GetType().Name, Status, Message);

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

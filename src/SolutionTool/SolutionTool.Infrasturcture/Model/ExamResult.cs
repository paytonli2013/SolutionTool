namespace Orc.SolutionTool
{
    public class ExamResult
    {
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
                }
            }
        }

        private Result? _result;
        public Result? Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                }
            }
        }
    }

    public enum Result
    {
        Passed,
        Fail
    }
}

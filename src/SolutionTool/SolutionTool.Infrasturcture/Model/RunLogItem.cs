using System;
using System.Xml.Serialization;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("run")]
    public class RunLogItem
    {
        [XmlElement("project")]
        public string Project { get; set; }

        [XmlElement("rules")]
        public int? Rules { get; set; }

        [XmlElement("start")]
        public DateTime? Start { get; set; }

        [XmlElement("end")]
        public DateTime? End { get; set; }

        [XmlIgnore]
        ExamResult _result;

        [XmlElement("result")]
        public Result? Result
        {
            get
            {
                var rv = _result == null ? null : (Result?)_result.Result;

                return rv;
            }
            set
            {
                if (_result == null)
                {
                    _result = new ExamResult();
                }

                _result.Result = value;
            }
        }

        [XmlElement("summary")]
        public String Summary
        {
            get
            {
                var rv = _result == null ? null : _result.Summary;

                if (rv == null && _result != null && _result.Result.HasValue && Start.HasValue && End.HasValue)
                {
                    rv = string.Format("{0} ({1} rule{2}) {3} < {4}s",
                        Project, Rules, (Rules ?? 0) > 0 ? "s" : "", _result.Result, (End.Value - Start.Value).TotalSeconds
                        );
                }

                return rv;
            }
            set
            {
                if (_result == null)
                {
                    _result = new ExamResult();
                }

                _result.Summary = value;
            }
        }

        [XmlElement("report")]
        public string Report { get; set; }
    }
}

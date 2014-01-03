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

        private Result _result;

        [XmlElement("status")]
        public ActionStatus Status
        {
            get
            {
                if (_result == null)
                {
                    return ActionStatus.None;
                }

                return _result.Status;
            }
            set
            {
                if (_result == null)
                {
                    _result = new ExamResult();
                }

                _result.Status = value;
            }
        }

        [XmlElement("summary")]
        public String Summary
        {
            get
            {
                var rv = _result == null ? null : _result.Summary;

                if (rv == null && _result != null && _result.Summary != null && Start.HasValue && End.HasValue)
                {
                    rv = string.Format("{0} ({1} rule{2}) {3} < {4}s",
                        Project, Rules, (Rules ?? 0) > 0 ? "s" : "", _result.Summary, (End.Value - Start.Value).TotalSeconds
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

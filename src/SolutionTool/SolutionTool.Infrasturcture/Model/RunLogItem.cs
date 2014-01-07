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

        [XmlElement("report")]
        public string Report { get; set; }
    }
}

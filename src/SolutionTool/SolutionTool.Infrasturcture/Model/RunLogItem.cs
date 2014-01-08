using System;
using System.Xml.Serialization;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("run")]
    public class RunLogItem
    {
        [XmlAttribute("project")]
        public string Project { get; set; }

        [XmlAttribute("rules")]
        public int? Rules { get; set; }

        [XmlAttribute("start")]
        public DateTime? Start { get; set; }

        [XmlAttribute("end")]
        public DateTime? End { get; set; }

        private Result _result;

        [XmlAttribute("status")]
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

        public string Summary
        {
            get
            {
                return string.Format("{0} rules checked in {1}sec, Status:{1}", Rules, 0.1, Status.ToString());
            }
        }
    }
}

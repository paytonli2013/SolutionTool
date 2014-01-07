using System;
using System.Xml.Serialization;
using Orc.SolutionTool.Mvvm;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("project")]
    public class Project : NotificationObject
    {
        string _name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        string _path;
        [XmlAttribute("path")]
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged(() => Path);
            }
        }
        private DateTime _createTime;
        [XmlAttribute("createTime")]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged(() => CreateTime);
            }
        }


        RuleSet _ruleSet;
        [XmlElement("ruleSet")]
        public RuleSet RuleSet
        {
            get { return _ruleSet; }
            set
            {
                _ruleSet = value;
                RaisePropertyChanged("TargetPath");
            }
        }
    }
}

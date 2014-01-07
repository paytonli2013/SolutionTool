using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class Project : NotificationObject
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        string _ruleSetPath;

        public string RuleSetPath
        {
            get { return _ruleSetPath; }
            set
            {
                _ruleSetPath = value;
                RaisePropertyChanged("RuleSetPath");
            }
        }

        string _targetPath;

        public string TargetPath
        {
            get { return _targetPath; }
            set
            {
                _targetPath = value;
                RaisePropertyChanged("TargetPath");
            }
        }

        IRuleSet _ruleSet;
        public IRuleSet RuleSet
        {
            get
            {
                return _ruleSet;
            }
            private set
            {
                _ruleSet = value;
                RaisePropertyChanged("RuleSetPath");
            }
        }

        private DateTime createTime;

        public DateTime CreateTime
        {
            get { return createTime; }
            set
            {
                createTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }
    }
}

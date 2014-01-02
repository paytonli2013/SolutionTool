using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SolutionChecker
{
    public class SolutionCheckerViewmodel : ViewmodelBase
    {
        IProjectManager _projectManager;
        IRuleRunner _ruleRunner;
        public SolutionCheckerViewmodel(IShellService shellService, IProjectManager projectManager, IRuleRunner ruleRunner)
            : base(shellService)
        {
            _projectManager = projectManager;
            _ruleRunner = ruleRunner;

            _projectManager.LoadProjects(OnLoaded);

            _ruleRunner.LoadRunLog(OnLogLoaded);
        }

        ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                RaisePropertyChanged("Projects");
            }
        }

        ObservableCollection<RunLogItem> _recentRun;

        public ObservableCollection<RunLogItem> RecentRun
        {
            get { return _recentRun; }
            set
            {
                _recentRun = value;
                RaisePropertyChanged("RecentRun");
            }
        }

        public void OnLoaded(IEnumerable<Project> projects, Exception error)
        {
            if (error != null)
            {

            }
            else
            {
                Projects = new ObservableCollection<Project>(projects);
            }
        }

        public void OnLogLoaded(IEnumerable<RunLogItem> log, Exception error)
        {
            if (error != null)
            {

            }
            else if (log != null)
            {
                RecentRun = new ObservableCollection<RunLogItem>(log);
            }
        }
    }
}

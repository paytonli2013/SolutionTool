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
        public SolutionCheckerViewmodel(IShellService shellService, IProjectManager projectManager)
            : base(shellService)
        {
            _projectManager = projectManager;

            _projectManager.LoadProejcts(OnLoaded);
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;

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

            _projectManager.Load(OnLoaded);

            _ruleRunner.LoadRunLog(OnLogLoaded);

            _ruleRunner.RunLogAdded += _ruleRunner_RunLogAdded;
        }

        void _ruleRunner_RunLogAdded(object sender, RunLogEventArgs e)
        {
            RecentRun.Insert(0, e.Item);
        }

        Project _selectedProject;

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                _selectedProject = value;
                RaisePropertyChanged("SelectedProject");
                RunProjectCommand.RaiseCanExecuteChanged();
                DeleteProjectCommand.RaiseCanExecuteChanged();
            }
        }

        RunLogItem _selectedRunLog;

        public RunLogItem SelectedRunLog
        {
            get { return _selectedRunLog; }
            set
            {
                _selectedRunLog = value;
                RaisePropertyChanged("SelectedRunLog");
                ViewReportCommand.RaiseCanExecuteChanged();
            }
        }

        ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                RaisePropertyChanged("Projects");
                DeleteProjectCommand.RaiseCanExecuteChanged();
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
                ClearRunLogCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnLoaded(IEnumerable<Project> projects, Exception error)
        {
            if (error != null)
            {
                _shellService.MessageService.Show(error.Message);
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
                _shellService.MessageService.Show(error.Message);
            }
            else if (log != null)
            {
                RecentRun = new ObservableCollection<RunLogItem>(log);
            }
        }

        #region Commands

        public DelegateCommand CreateProjectCommand { get { return new DelegateCommand(ExecCreate, CanExecCreate); } }

        void ExecCreate()
        {
            try
            {
                _shellService.OpenChildView("NewProjectView", "New Project Wizard", (result) =>
                {
                    if (result.HasFlag(CloseResult.Success))
                        _shellService.PostStatusMessage(StatusCatgory.None, "Project Created!");

                    if (result.HasFlag(CloseResult.Cancel))
                        _shellService.PostStatusMessage(StatusCatgory.None, "Canceled");

                    if (result.HasFlag(CloseResult.RefreshParent))
                    {
                        Refresh();
                    }

                }, new ViewOptions { Height = 380, Width = 500 });
            }
            catch (Exception ex)
            {
                _shellService.MessageService.Show(ex.Message);
            }
        }

        bool CanExecCreate()
        {
            return true;
        }

        DelegateCommand viewReportCommand;
        public DelegateCommand ViewReportCommand
        {
            get
            {
                if (viewReportCommand == null)
                    viewReportCommand = new DelegateCommand(ExecViewReport, CanExecViewReport);
                return viewReportCommand;
            }
        }

        void ExecViewReport()
        {
            try
            {
                _shellService.OpenChildView("ReportViewer", "Report Viewer", (result) =>
                {
                    //if (result.HasFlag(CloseResult.Success))
                    //    _shellService.PostStatusMessage(StatusCatgory.None, "Project Created!");

                    //if (result.HasFlag(CloseResult.Cancel))
                    //    _shellService.PostStatusMessage(StatusCatgory.None, "Canceled");

                    //if (result.HasFlag(CloseResult.RefreshParent))
                    //{
                    //    Refresh();
                    //}
                }, new ViewOptions { Height = 600, Width = 800, Payload = SelectedRunLog });
            }
            catch (Exception ex)
            {
                _shellService.MessageService.Show(ex.Message);
            }
        }

        bool CanExecViewReport()
        {
            return SelectedRunLog != null;
        }

        DelegateCommand runProjectCommand;
        public DelegateCommand RunProjectCommand
        {
            get
            {
                if (runProjectCommand == null)
                    runProjectCommand = new DelegateCommand(ExecRun, CanExecRun);
                return runProjectCommand;
            }
        }

        void ExecRun()
        {
            _shellService.PostStatusMessage(StatusCatgory.Info, string.Format("Running {0}", SelectedProject.Name));

            RunCodeInUiThread(() =>
            {
                _ruleRunner.RunProject(SelectedProject, OnRunComplete);
            });
        }

        private void OnRunComplete(Report report,RunLogItem logitem)
        {
            _shellService.PostStatusMessage(StatusCatgory.Info, "Complete");

            RunCodeInUiThread(() =>
            {
                _shellService.OpenChildView("ReportViewer", "Report Viewer", option: new ViewOptions { Height = 600, Width = 800, Payload = logitem });
            });
        }

        bool CanExecRun()
        {
            return SelectedProject != null;
        }

        #region DeleteProject Command

        public DelegateCommand<object> _deleteProjectCommand;
        public DelegateCommand<object> DeleteProjectCommand
        {
            get
            {
                return _deleteProjectCommand 
                    ?? (_deleteProjectCommand = new DelegateCommand<object>(DeleteProject, CanDeleteProject));
            }
            set
            {
                _deleteProjectCommand = value;
            }
        }

        private bool CanDeleteProject(object arg)
        {
            return SelectedProject != null && Projects != null && Projects.Count > 0;
        }

        private void DeleteProject(object arg)
        {
            _projectManager.Delete(SelectedProject, OnDeleteProjectComplete);
        }

        private void OnDeleteProjectComplete(bool ok, Exception exception)
        {
            if (exception != null)
            {
                _shellService.MessageService.Show(exception.Message);
            }
            else if (ok)
            {
                Projects.Remove(SelectedProject);
                SelectedProject = null;
            }
        }

        #endregion

        #region ClearRunLog Command

        public DelegateCommand<object> _clearRunLogCommand;
        public DelegateCommand<object> ClearRunLogCommand
        {
            get
            {
                return _clearRunLogCommand 
                    ?? (_clearRunLogCommand = new DelegateCommand<object>(ClearRunLog, CanClearRunLog));
            }
            set
            {
                _clearRunLogCommand = value;
            }
        }

        private bool CanClearRunLog(object arg)
        {
            return RecentRun != null && RecentRun.Count > 0;
        }

        private void ClearRunLog(object arg)
        {
            _ruleRunner.ClearLog(OnClearLogComplete);
        }

        private void OnClearLogComplete(Exception exception)
        {
            if (exception != null)
            {
                _shellService.MessageService.Show(exception.Message);
            }
            else
            {
                RecentRun.Clear();
            }
        }

        #endregion

        #endregion

        private void Refresh()
        {
            //throw new NotImplementedException();
            _projectManager.Load(OnLoaded);
        }
    }
}

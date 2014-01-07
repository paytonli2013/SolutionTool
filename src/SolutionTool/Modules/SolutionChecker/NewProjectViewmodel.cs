using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
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
    public class NewProjectViewmodel : ViewmodelBase
    {
        #region Constructor

        public NewProjectViewmodel(IShellService shellService, IProjectManager projectManager, IRuleManager ruleManager)
            : base(shellService)
        {
            _projectManager = projectManager;
            _ruleManager = ruleManager;
            Init();
        }

        #endregion

        #region Commands & Methods

        private void Init()
        {
            //throw new NotImplementedException();
            Project = new Project() {};

            _ruleManager.LoadRuleSet(OnRuleSetLoaded);
        }

        private void OnRuleSetLoaded(IEnumerable<IRuleSet> ruleSets, Exception error)
        {
            if (error != null)
            {
                _shellService.MessageService.Show(error.Message);
            }
            else
            {
                RuleSetTemplates = new ObservableCollection<IRuleSet>(ruleSets);

                var first = RuleSetTemplates.FirstOrDefault();

                if (first != null)
                {
                    RuleSet = first;
                }
            }
        }

        DelegateCommand createCommand;

        public DelegateCommand CreateCommand
        {
            get
            {
                if (createCommand == null)
                    createCommand = new DelegateCommand(ExecCreate, CanExecCreate);
                return createCommand;
            }
        }

        void ExecCreate()
        {
            try
            {
                _projectManager.Create(_project, OnCreateComplete);
            }
            catch (Exception ex)
            {
                _shellService.MessageService.Show(ex.Message);
            }
        }

        bool CanExecCreate()
        {
            return !string.IsNullOrEmpty(TargetPath) && !string.IsNullOrEmpty(ProjectName) && RuleSet != null;
        }

        public DelegateCommand CancelCommand { get { return new DelegateCommand(ExecCancel, CanExecCancel); } }

        void ExecCancel()
        {
            try
            {
                _shellService.Host.Close(CloseResult.Cancel);
            }
            catch (Exception ex)
            {
                _shellService.MessageService.Show(ex.Message);
            }
        }

        bool CanExecCancel()
        {
            return true;
        }

        public DelegateCommand OpenFolderCommand { get { return new DelegateCommand(ExecOpenFolder, CanExecOpenFolder); } }

        void ExecOpenFolder()
        {
            try
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();

                var result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                    TargetPath = dlg.SelectedPath;
            }
            catch (Exception ex)
            {
                _shellService.MessageService.Show(ex.Message);
            }
        }

        bool CanExecOpenFolder()
        {
            return true;
        }

        void OnCreateComplete(Project project, Exception ex)
        {
            if (ex != null)
            {
                _shellService.MessageService.Show(ex.Message);
            }
            else
            {
                _shellService.Host.Close(CloseResult.Success | CloseResult.RefreshParent);
            }

        }

        #endregion

        #region Properties

        Project _project;

        public Project Project
        {
            get { return _project; }
            set
            {
                _project = value;
                RaisePropertyChanged("Project");
            }
        }

        public string ProjectName
        {
            get
            {
                return _project.Name;
            }
            set
            {
                _project.Name = value;
                RaisePropertyChanged("ProjectName");
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        public string TargetPath
        {
            get { return _project.TargetPath; }
            set
            {
                _project.TargetPath = value;
                RaisePropertyChanged("TargetPath");
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        IProjectManager _projectManager;
        IRuleManager _ruleManager;

        IRuleSet ruleSet;

        public IRuleSet RuleSet
        {
            get
            {
                return ruleSet;
            }
            set
            {
                ruleSet = value;
                RaisePropertyChanged("RuleSet");
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        ObservableCollection<IRuleSet> ruleSetTemplates;

        public ObservableCollection<IRuleSet> RuleSetTemplates
        {
            get { return ruleSetTemplates; }
            set
            {
                ruleSetTemplates = value;
                RaisePropertyChanged("RuleSetTemplates");
            }
        }

        #endregion
    }
}

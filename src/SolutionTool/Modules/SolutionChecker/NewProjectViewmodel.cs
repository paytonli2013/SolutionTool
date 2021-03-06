﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;

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

        private void OnRuleSetLoaded(IEnumerable<RuleSet> ruleSets, Exception error)
        {
            if (error != null)
            {
                _shellService.MessageService.Show(error.Message);
            }
            else
            {
                RuleSetTemplates = new ObservableCollection<RuleSet>(ruleSets);

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
                var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settingOfInspectCodeExePath = cfg.AppSettings.Settings["InspectCodeExePath"];

                if (settingOfInspectCodeExePath != null
                    && !string.IsNullOrWhiteSpace(settingOfInspectCodeExePath.Value))
                {
                    var v = settingOfInspectCodeExePath.Value.Trim().ToLower();
                    var r = _project.RuleSet.OfType<CodeAnalysisRule>().FirstOrDefault(x => x is CodeAnalysisRule);

                    if (System.IO.File.Exists(v))
                    {
                        r.Path = v;
                    }
                }

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
                {
                    TargetPath = dlg.SelectedPath;
                    if (string.IsNullOrEmpty(ProjectName))
                    {
                        ProjectName = TargetPath.Substring(TargetPath.LastIndexOf("\\")).TrimStart('\\');
                    }
                }
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
            get { return _project.Path; }
            set
            {
                _project.Path = value;
                RaisePropertyChanged("TargetPath");
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        IProjectManager _projectManager;
        IRuleManager _ruleManager;

        RuleSet ruleSet;

        public RuleSet RuleSet
        {
            get
            {
                return ruleSet;
            }
            set
            {
                ruleSet = value;
                _project.RuleSet = ruleSet;
                RaisePropertyChanged("RuleSet");
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        ObservableCollection<RuleSet> ruleSetTemplates;

        public ObservableCollection<RuleSet> RuleSetTemplates
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

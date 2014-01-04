using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Orc.SolutionTool.Model;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace ManageTemplate
{
    public class ManageTemplateViewmodel : ViewmodelBase
    {
        private ITemplateManager _templateMgr;

        private ObservableCollection<string> _templateFiles = new ObservableCollection<string>();
        public ObservableCollection<string> TemplateFiles
        {
            get
            {
                return _templateFiles;
            }
            set
            {
                if (_templateFiles != value)
                {
                    _templateFiles = value;
                    RaisePropertyChanged(() => TemplateFiles);
                }
            }
        }

        private string _selectedTemplateFile;
        public string SelectedTemplateFile
        {
            get
            {
                return _selectedTemplateFile;
            }
            set
            {
                if (_selectedTemplateFile != value)
                {
                    _selectedTemplateFile = value;
                    RaisePropertyChanged(() => SelectedTemplateFile);

                    ReloadRepository();
                }
            }
        }

        private string _addingNewText = "--Add a New Template--";
        public string AddingNewText
        {
            get
            {
                return _addingNewText;
            }
            set
            {
                if (_addingNewText != value)
                {
                    _addingNewText = value;
                    RaisePropertyChanged(() => AddingNewText);
                }
            }
        }

        private Repository _repository;
        public Repository Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    RaisePropertyChanged(() => Repository);
                }
            }
        }

        private Target _selectedTarget;
        public Target SelectedTarget
        {
            get
            {
                return _selectedTarget;
            }
            set
            {
                if (_selectedTarget != value)
                {
                    _selectedTarget = value;
                    RaisePropertyChanged(() => SelectedTarget);
                }
            }
        }

        #region AddTarget Command

        public DelegateCommand<object> _addTargetCommand;
        public DelegateCommand<object> AddTargetCommand
        {
            get
            {
                return _addTargetCommand ?? (_addTargetCommand = new DelegateCommand<object>(AddTarget, CanAddTarget));
            }
            set
            {
                _addTargetCommand = value;
            }
        }

        private bool CanAddTarget(object arg)
        {
            return true;
        }

        private void AddTarget(object arg)
        {
#if DEBUG
            var sf = new System.Diagnostics.StackFrame();

            System.Windows.MessageBox.Show(sf.GetMethod().Name + (arg != null ? arg.ToString() : null));
#endif
        }

        #endregion

        #region RemoveTarget Command

        public DelegateCommand<object> _removeTargetCommand;
        public DelegateCommand<object> RemoveTargetCommand
        {
            get
            {
                return _removeTargetCommand ?? (_removeTargetCommand = new DelegateCommand<object>(RemoveTarget, CanRemoveTarget));
            }
            set
            {
                _removeTargetCommand = value;
            }
        }

        private bool CanRemoveTarget(object arg)
        {
            return true;
        }

        private void RemoveTarget(object arg)
        {
#if DEBUG
            var sf = new System.Diagnostics.StackFrame();

            System.Windows.MessageBox.Show(sf.GetMethod().Name + (arg != null ? arg.ToString() : null));
#endif
        }

        #endregion

        public ManageTemplateViewmodel(IShellService shellService, ITemplateManager templateMgr)
            : base(shellService)
        {
            _templateMgr = templateMgr;

            InitializeData();
        }

        private void InitializeData()
        {
            _templateMgr.GetAllTemplateFileNames((x, y) =>
            {
                if (y != null)
                {
                    _shellService.PostStatusMessage(StatusCatgory.Error, y.Message);
                }

                if (x != null)
                {
                    foreach (var i in x)
                    {
                        TemplateFiles.Add(i);
                    }

                    TemplateFiles.Insert(0, AddingNewText);
                }
            });

            Repository = new Repository(@"..\..\..\");
        }

        private void ReloadRepository()
        {
            if (SelectedTemplateFile != AddingNewText)
            {
                _templateMgr.LoadTemplate(SelectedTemplateFile, (x, y) =>
                {
                    if (y != null)
                    {
                        _shellService.PostStatusMessage(StatusCatgory.Error, y.Message);
                    }

                    Repository = x;
                });
            }
        }

    }
}

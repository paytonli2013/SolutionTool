using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;

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

        private string _templateFile;
        public string TemplateFile
        {
            get
            {
                return _templateFile;
            }
            set
            {
                if (_templateFile != value)
                {
                    _templateFile = value;

                    if (_templateFile != null && !_templateFile.EndsWith(".xml"))
                    {
                        _templateFile += ".xml";
                    }

                    RaisePropertyChanged(() => TemplateFile);
                    SaveCommand.RaiseCanExecuteChanged();
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

        private string _templateXmlContent;
        public string TemplateXmlContent
        {
            get
            {
                return _templateXmlContent;
            }
            set
            {
                if (_templateXmlContent != value)
                {
                    _templateXmlContent = value;
                    RaisePropertyChanged("TemplateXmlContent");
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #region Save Command

        public DelegateCommand<object> _saveCommand;
        public DelegateCommand<object> SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand<object>(Save, CanSave));
            }
            set
            {
                _saveCommand = value;
            }
        }

        private bool CanSave(object arg)
        {
            //string.Compare(TemplateFile, SelectedTemplateFile, StringComparison.OrdinalIgnoreCase) == 0 || 
            if (string.IsNullOrWhiteSpace(TemplateFile)
                || string.IsNullOrWhiteSpace(TemplateXmlContent)
                || Path.GetInvalidFileNameChars().Any(x => TemplateFile.IndexOf(x) > -1))
            {
                return false;
            }

            return true;
        }

        private void Save(object arg)
        {
            _shellService.PostStatusMessage(StatusCatgory.None, "Saving Template");

            RunCodeInUiThread(() =>
            {
                _templateMgr.SaveTemplate(TemplateFile, TemplateXmlContent, (x, y) =>
                {
                    if (y != null)
                    {
                        _shellService.PostStatusMessage(StatusCatgory.Error, y.Message);
                    }
                    else
                    {
                        _shellService.PostStatusMessage(StatusCatgory.None, "Template Saved!");
                    }

                    if (x && !TemplateFiles.Any(z => string.Compare(z, TemplateFile, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        TemplateFiles.Add(TemplateFile);
                        SelectedTemplateFile = TemplateFile;
                    }
                });
            });
        }

        #endregion

        #region Delete Command

        public DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new DelegateCommand<object>(Delete, CanDelete));
            }
            set
            {
                _deleteCommand = value;
            }
        }

        private bool CanDelete(object arg)
        {
            return !string.IsNullOrWhiteSpace(SelectedTemplateFile);
        }

        private void Delete(object arg)
        {
            _shellService.MessageService.Confirm("are you sure to delete this template?",(r) =>
            {
                if (r == MessageBoxResult.Yes)
                {
                    _templateMgr.DeleteTemplate(SelectedTemplateFile, (x, y) =>
                    {
                        if (y != null)
                        {
                            _shellService.PostStatusMessage(StatusCatgory.Error, y.Message);
                        }

                        if (x)
                        {
                            TemplateFiles.Remove(SelectedTemplateFile);
                            TemplateFile = null;
                            TemplateXmlContent = null;
                            SelectedTemplateFile = TemplateFiles.LastOrDefault();
                        }
                    });
                }
            });

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
                }
            });
        }

        private void ReloadRepository()
        {
            _templateMgr.LoadTemplate(SelectedTemplateFile, (x, y, z) =>
            {
                if (z != null)
                {
                    _shellService.PostStatusMessage(StatusCatgory.Error, z.Message);
                }

                TemplateFile = SelectedTemplateFile;
                TemplateXmlContent = y;
                DeleteCommand.RaiseCanExecuteChanged();
            });
        }

    }
}

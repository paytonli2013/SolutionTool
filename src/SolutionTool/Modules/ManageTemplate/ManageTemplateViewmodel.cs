using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Orc.SolutionTool.Model;
using System.ComponentModel;

namespace ManageTemplate
{
    public class ManageTemplateViewmodel : ViewmodelBase
    {
        private ObservableCollection<string> _templateFiles;
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

        public ManageTemplateViewmodel(IShellService shellService)
            : base(shellService)
        {
            Repository = new Repository(@"..\..\..\");
        }
    }
}

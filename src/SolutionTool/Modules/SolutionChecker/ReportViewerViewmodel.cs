using Microsoft.Practices.Prism.Commands;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolutionChecker
{
    public class ReportViewerViewmodel : ViewmodelBase, IChildView
    {
        public ReportViewerViewmodel(IShellService shellService)
            : base(shellService)
        {

        }

        #region Properties

        RunLogItem _runlog;

        public RunLogItem Runlog
        {
            get { return _runlog; }
            set
            {
                _runlog = value;
                RaisePropertyChanged("Runlog");
            }
        }
        string _reportText;

        public string ReportText
        {
            get { return _reportText; }
            set
            {
                _reportText = value; RaisePropertyChanged("ReportText");
                SaveAsCommand.RaiseCanExecuteChanged();
                CopyCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Command
        DelegateCommand saveAsCommand;
        public DelegateCommand SaveAsCommand
        {
            get
            {
                if (saveAsCommand == null)
                    saveAsCommand = new DelegateCommand(ExecSaveAs, CanExecSaveAs);
                return saveAsCommand;
            }
        }

        void ExecSaveAs()
        {

        }

        bool CanExecSaveAs()
        {
            return !string.IsNullOrEmpty(ReportText);
        }

        DelegateCommand copyCommand;
        public DelegateCommand CopyCommand
        {
            get
            {
                if (copyCommand == null)
                    copyCommand = new DelegateCommand(ExecCopy, CanExecCopy);
                return copyCommand;
            }
        }

        void ExecCopy()
        {

        }

        bool CanExecCopy()
        {
            return !string.IsNullOrEmpty(ReportText);
        }

        #endregion

        public void SetPayload(object data)
        {
            var runlog = data as RunLogItem;
            if (runlog != null)
            {
                Runlog = runlog;
                ReportText = Runlog.Report;
            }
            //throw new NotImplementedException();
        }
    }
}

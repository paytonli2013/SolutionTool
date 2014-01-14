using System;
using System.IO;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;

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
            try
            {
                var fileName = string.Format("Check_solution_{0}.txt", Runlog.Project);
                var dlg = new SaveFileDialog() { Filter = "All files|*.txt", Title = "Select file name", FileName = fileName };
                if (dlg.ShowDialog() == true)
                {
                    using (var stream = dlg.OpenFile())
                    using (var sr = new StreamWriter(stream))
                    {
                        sr.Write(ReportText);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            Clipboard.SetText(ReportText);
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
                ReportText = Report.GetTextFile(Runlog.Report);
                ReportHtml = Report.GetHtmlFile(Runlog.Report);
            }
        }

        string _reportText;

        public string ReportText
        {
            get
            {
                return _reportText;
            }
            private set
            {
                _reportText = value;
                RaisePropertyChanged("ReportText");
            }
        }

        string _reportHtml;
        public string ReportHtml
        {
            get
            {
                return _reportHtml;
            }
            private set
            {
                _reportHtml = value;
                RaisePropertyChanged("ReportHtml");
            }
        }
    }
}

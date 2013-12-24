using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StyleCop;
using StyleCop.CSharp;

namespace Orc.StyleCop.CSharp.Rules
{
    [SourceAnalyzer(typeof(CsParser))]
    public class FoldersMustExist : SourceAnalyzer
    {
        string _projectDir;

        public FoldersMustExist()
        {
            this.Core.ProjectSettingsChanged += Core_ProjectSettingsChanged;
        }

        void Core_ProjectSettingsChanged(object sender, EventArgs e)
        {

        }

        public override void AnalyzeDocument(CodeDocument document)
        {
            //if (!string.IsNullOrEmpty(_projectDir))
            //{
            //    return;
            //}

            System.Diagnostics.Debugger.Break();

            _projectDir = document.SourceCode.Project.Location;

            var folderList = this.GetSetting(document.Settings, "FolderList") as CollectionProperty;
            var folders = new List<string>();

            foreach (var i in folderList)
            {
                var path = Path.Combine(_projectDir, i);

                if (!Directory.Exists(path))
                {
                    folders.Add(i);
                }
            }

            if (folders.Count > 0)
            {
                var s = string.Join(", ", folders.ToArray());

                AddViolation(document.DocumentContents, "FoldersMustExist", s);
            }
        }
    }
}

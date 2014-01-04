using System;
using System.Collections.Generic;

namespace Orc.SolutionTool.Model
{
    public interface ITemplateManager
    {
        void GetAllTemplateFileNames(Action<IEnumerable<string>, Exception> onComplate);
        void LoadTemplate(string templateFileName, Action<Repository, Exception> onComplete);
        void SaveTemplate(string templateFileName, Repository repository, Action<bool, Exception> onComplete);
    }
}

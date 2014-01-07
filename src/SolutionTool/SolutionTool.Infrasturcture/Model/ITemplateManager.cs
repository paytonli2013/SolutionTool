using System;
using System.Collections.Generic;

namespace Orc.SolutionTool.Model
{
    public interface ITemplateManager
    {
        void GetAllTemplateFileNames(Action<IEnumerable<string>, Exception> onComplate);
        void LoadTemplate(string templateFileName, Action<Directory, string, Exception> onComplete);
        void SaveTemplate(string templateFileName, string templateXmlContent, Action<bool, Exception> onComplete);
        void DeleteTemplate(string templateFileName, Action<bool, Exception> onComplete);
        void ValidateTemplate(string templateXmlContent, Action<bool, Exception> onComplete);
    }
}

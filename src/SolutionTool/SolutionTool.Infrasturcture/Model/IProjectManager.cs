using System;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public interface IProjectManager
    {
        void Load(Action<System.Collections.Generic.IEnumerable<Project>, Exception> onComplete);
        void Create(Project project, Action<Project, Exception> onComplete);
        void Update(Project project, Action<Project, Exception> onComplete);
        void Delete(Project project, Action<bool, Exception> onComplete);
    }
}

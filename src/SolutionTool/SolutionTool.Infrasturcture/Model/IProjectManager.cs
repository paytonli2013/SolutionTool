using Orc.SolutionTool.Model;
using System;
namespace Orc.SolutionTool
{
    public interface IProjectManager
    {
        void Load(Action<System.Collections.Generic.IEnumerable<Project>, Exception> onComplete);
        void Create(Project project, Action<Project, Exception> onComplete);
        void Update(Project project, Action<Project, Exception> onComplete);

    }
}

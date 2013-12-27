using Orc.SolutionTool.Model;
using System;
namespace Orc.SolutionTool
{
    public interface IProjectManager
    {
        void LoadProejcts(Action<System.Collections.Generic.IEnumerable<Project>, Exception> onComplete);
        void UpdateProject(Project project);
    }
}

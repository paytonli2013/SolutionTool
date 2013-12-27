using Orc.SolutionTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public class ProjectManager : IProjectManager
    {
        public void LoadProejcts(Action<IEnumerable<Project>, Exception> onComplete)
        {
            //throw new NotImplementedException();
            if (onComplete != null)
            {
                onComplete(BuildProjects(), null);
            }
        }

        private IEnumerable<Project> BuildProjects()
        {
            List<Project> list = new List<Project>();
            
            list.Add(new Project
            {
                Name = "Demo Project",
                TargetPath = "E:\\Tools", 
            });

            list.Add(new Project
            {
                Name = "Demo Project",
                TargetPath = "E:\\Tools", 
            });
            
            list.Add(new Project
            {
                Name = "Demo Project",
                TargetPath = "E:\\Tools", 
            });

            list.Add(new Project
            {
                Name = "Demo Project",
                TargetPath = "E:\\Tools", 
            });
            
            list.Add(new Project
            {
                Name = "Demo Project",
                TargetPath = "E:\\Tools",
            });

            return list;
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}

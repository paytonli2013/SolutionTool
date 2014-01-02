using Orc.SolutionTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Orc.SolutionTool
{
    public class ProjectManager : IProjectManager
    {
        public void LoadProjects(Action<IEnumerable<Project>, Exception> onComplete)
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

            var cd = Environment.CurrentDirectory;
            var dir = System.IO.Path.Combine(cd, @".\Projects\");

            if (!System.IO.Directory.Exists(dir))
            {
                return list;
            }

            foreach (var i in System.IO.Directory.GetFiles(dir, "*.xml"))
            {
                var xdoc = XDocument.Load(i);
                var attrName = xdoc.Root.Attribute("name");
                var attrRuleSet = xdoc.Root.Attribute("ruleset");
                var attrTarget = xdoc.Root.Attribute("target");
                var prj = new Project 
                {
                    Name = attrName == null ? null : attrName.Value,
                    RuleSetPath = attrRuleSet == null ? null : attrRuleSet.Value,
                    TargetPath = attrTarget == null ? null : attrTarget.Value,
                };
            }

            return list;
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}

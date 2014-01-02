using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public class ProjectManager : IProjectManager
    {
        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Projects\");

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

            if (!System.IO.Directory.Exists(_dir))
            {
                return list;
            }

            foreach (var i in System.IO.Directory.GetFiles(_dir, "*.xml"))
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

                list.Add(prj);
            }

            return list;
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}

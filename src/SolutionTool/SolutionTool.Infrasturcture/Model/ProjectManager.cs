using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public class ProjectManager : IProjectManager
    {
        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Projects\");

        public void Load(Action<IEnumerable<Project>, Exception> onComplete)
        {
            //throw new NotImplementedException();
            if (onComplete != null)
            {
                onComplete(BuildProjects(), null);
            }
        }

        List<Project> list;
        private IEnumerable<Project> BuildProjects()
        {
            if (list != null)
                return list;
            else
                list = new List<Project>();

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

            //list.Sort(new Comparison<Project>((p1, p2) =>
            //{
            //    if (p1 == p2)
            //        return 0;
            //    if (p1.CreateTime > p2.CreateTime)
            //        return -1;
            //    else
            //        return 1;
            //}));

            return list;
        }

        public void Create(Project project, Action<Project, Exception> onComplete)
        {
            if (project.CreateTime == DateTime.MinValue)
                project.CreateTime = DateTime.Now;

            list.Insert(0,project);

            if (onComplete != null)
            {
                onComplete.Invoke(project, null);
            }
            //throw new NotImplementedException();
        }

        public void Update(Project project, Action<Project, Exception> onComplete)
        {
            if (onComplete != null)
            {
                onComplete.Invoke(project, null);
            }
            //throw new NotImplementedException();
        }
    }
}

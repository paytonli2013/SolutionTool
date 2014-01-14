using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public class ProjectManager : IProjectManager
    {
        private static readonly string _dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Projects\");

        public void Load(Action<IEnumerable<Project>, Exception> onComplete)
        {
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

            if (!Directory.Exists(_dir))
            {
                return list;
            }

            foreach (var i in Directory.GetFiles(_dir, "*.xml"))
            {
                var xs = new XmlSerializer(typeof(Project));

                using (var xr = XmlReader.Create(i))
                {
                    var prj = xs.Deserialize(xr) as Project;

                    list.Add(prj);
                }
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

            var rv = list.OrderByDescending(x => x.CreateTime).ToList();

            return rv;
        }

        public void Create(Project project, Action<Project, Exception> onComplete)
        {
            var exception = null as Exception;

            try
            {
                SaveProject(project);
            }
            catch (Exception xe)
            {
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete.Invoke(project, exception);
            }
        }

        public void Update(Project project, Action<Project, Exception> onComplete)
        {
            var exception = null as Exception;

            try
            {
                SaveProject(project, overrideExist: true);
            }
            catch (Exception xe)
            {
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete.Invoke(project, exception);
            }
        }

        public void Delete(Project project, Action<bool, Exception> onComplete)
        {
            var exception = null as Exception;

            try
            {
                SaveProject(project, delete: true);
            }
            catch (Exception xe)
            {
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete.Invoke(exception == null, exception);
            }
        }

        private Exception SaveProject(Project project, bool overrideExist = false, bool delete = false)
        {
            if (project.CreateTime == DateTime.MinValue)
                project.CreateTime = DateTime.Now;

            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }

            var path = Path.Combine(_dir, project.Name + ".xml");
            var exception = null as Exception;

            if (delete && File.Exists(path))
            {
                File.Delete(path);
            }
            else if (File.Exists(path) && !overrideExist)
            {
                throw new Exception("Project [" + project.Name + "] already exists. ");
            }
            else
            {
                if (overrideExist)
                {
                    File.Delete(path);
                }

                using (var fs = XmlWriter.Create(path))
                {
                    var xs = new XmlSerializer(typeof(Project));

                    xs.Serialize(fs, project);
                }

                list.Insert(0, project);
            }

            return exception;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Orc.SolutionTool.Mvvm;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("repository")]
    public class Repository : NotificationObject
    {
        private string _path;
        [XmlAttribute("path")]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    RaisePropertyChanged(() => Path);
                }
            }
        }

        private Target _target;
        [XmlElement("target")]
        public Target Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    RaisePropertyChanged(() => Target);
                }
            }
        }

        public Repository()
        {

        }

        public Repository(string path)
        {
            var di = new DirectoryInfo(path);

            Path = di.FullName;
            Target = new Target(di.Name, null);

            UpdateChildren(Target, path);
        }

        public void Exam(Action<ExamResult> action, bool recursive = false)
        {
            Exam(Target, action, recursive);
        }

        public void Exam(Target target, Action<ExamResult> action, bool recursive = false)
        {
            if (target == null)
            {
                return;
            }

            foreach (var i in target.Rules)
            {
                i.Exam(new Context(this, target), action);
            }

            if (recursive)
            {
                foreach (var i in target.Children)
                {
                    Exam(i, action, recursive);
                }
            }
        }

        public void Apply(Action<ApplyResult> action, bool recursive = false)
        {
            Apply(Target, action, recursive);
        }

        public void Apply(Target target, Action<ApplyResult> action, bool recursive = false)
        {
            if (target == null)
            {
                return;
            }

            foreach (var i in target.Rules)
            {
                i.Apply(new Context(this, target), action);
            }

            if (recursive)
            {
                foreach (var i in target.Children)
                {
                    Apply(i, action, recursive);
                }
            }
        }

        public void AddRule(string p, Rule rule)
        {
            var target = FindOrCreateTarget(Target, p);

            if (target != null)
            {
                target.Rules.Add(rule);
            }
        }

        public void RemoveRule(string p, Rule rule)
        {
            var target = FindOrCreateTarget(Target, p);

            if (target != null && target.Rules.Contains(rule))
            {
                target.Rules.Remove(rule);
            }
        }

        public void RemoveRule(string p, Type ruleType)
        {
            var target = FindOrCreateTarget(Target, p);

            if (target != null)
            {
                var rule = null as Rule;

                do
                {
                    rule = target.Rules.FirstOrDefault(x => x.GetType() == ruleType);

                    if (rule == null)
                    {
                        break;
                    }

                    target.Rules.Remove(rule);
                } while (true);
            }
        }

        public void RemoveRule(string p)
        {
            var target = FindOrCreateTarget(Target, p);

            if (target != null)
            {
                while (target.Rules.Count != 0)
                {
                    target.Rules.RemoveAt(0);
                }
            }
        }

        public Target FindOrCreateTarget(string path)
        {
            var target = FindOrCreateTarget(Target, path);

            return target;
        }

        private Target FindOrCreateTarget(Target parent, string path)
        {
            if (parent == null)
            {
                return null;
            }

            var pathx = path.Replace('/', '\\');

            Target target = null;

            if (!System.IO.Path.IsPathRooted(pathx))
            {
                var ix = pathx.IndexOf('\\');
                var name = ix == -1 ? pathx : pathx.Substring(0, ix);

                if (".".Equals(name))
                {
                    if (ix == -1)
                    {
                        return parent;
                    }

                    target = FindOrCreateTarget(parent, pathx.Substring(ix + 1));

                    return target;
                }

                var t = parent.Children.FirstOrDefault(x => x.Name.Equals(name));

                if (t == null)
                {
                    t = new Target(name, parent);

                    parent.Children.Add(t);
                }

                if (ix == -1)
                {
                    return t;
                }

                target = FindOrCreateTarget(t, pathx.Substring(ix + 1));
            }

            return target;
        }

        private static void UpdateChildren(Target target, string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                foreach (var i in System.IO.Directory.GetDirectories(path))
                {
                    var name = new DirectoryInfo(i).Name;
                    var child = new Target(name, target);

                    target.Children.Add(child);

                    UpdateChildren(child, i);
                }

                foreach (var i in System.IO.Directory.GetFiles(path))
                {
                    var name = new FileInfo(i).Name;
                    var child = new Target(name, target);

                    target.Children.Add(child);
                }
            }
        }
    }
}

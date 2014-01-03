using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Orc.SolutionTool.Model
{
    public class RuleManager : IRuleManager
    {
        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Rules\");

        System.IO.FileSystemWatcher _fsw = new System.IO.FileSystemWatcher(_dir, "*.xml");

        public RuleManager()
        {
            _fsw.Changed += _fsw_Changed;

            LoadRules();
        }

        Dictionary<string, IEnumerable<IRule>> _ruleSets = new Dictionary<string, IEnumerable<IRule>>();
        public Dictionary<string, IEnumerable<IRule>> RuleSets
        {
            get
            {
                return _ruleSets;
            }
        }

        public void Persist(IEnumerable<IRule> ruleSet, Action<bool, Exception> onComplete)
        {
            throw new NotImplementedException();
        }

        void _fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            LoadRules(e.FullPath);
        }

        void LoadRules(string name = null)
        {
            _fsw.EnableRaisingEvents = false;

            var files = null as string[];

            if (name == null)
            {
                files = System.IO.Directory.GetFiles(_dir, "*.xml");
            }
            else if (System.IO.File.Exists(name))
            {
                files = new string[] { name, };
            }

            if (files == null)
            {
                return;
            }

            foreach (var i in files)
            {
                var fi = new System.IO.FileInfo(i);

                if (_ruleSets.ContainsKey(fi.Name))
                {
                    _ruleSets.Remove(fi.Name);
                }

                var rules = new List<IRule>();

                _ruleSets.Add(fi.Name, rules);

                var xdoc = XDocument.Load(fi.FullName);

                foreach (var j in xdoc.Root.Elements())
                {
                    // TODO:
                    rules.Add(new DirectoryExistedRule { });
                }
            }

            _fsw.EnableRaisingEvents = true;
        }
    }
}

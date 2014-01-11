using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

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

        public string DefaultRuleSetName
        {
            get
            {
                return "default.xml";
            }
        }

        Dictionary<string, RuleSet> _ruleSets = new Dictionary<string, RuleSet>();
        public Dictionary<string, RuleSet> RuleSets
        {
            get
            {
                return _ruleSets;
            }
        }

        public RuleSet DefaultRuleSet
        {
            get
            {
                var rules = _ruleSets.FirstOrDefault(x => x.Key == DefaultRuleSetName).Value;

                return rules;
            }
        }

        public void Persist(RuleSet ruleSet, Action<bool, Exception> onComplete)
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
                var key = fi.Name.ToLower();

                if (_ruleSets.ContainsKey(key))
                {
                    _ruleSets.Remove(key);
                }

                var ruleSet = null as RuleSet;

                using (var xr = XmlReader.Create(i))
                {
                    var xs = new XmlSerializer(typeof(RuleSet));

                    ruleSet = xs.Deserialize(xr) as RuleSet;
                }

                if (ruleSet != null)
                {
                    _ruleSets.Add(key, ruleSet);
                }
            }

            // Check for default ruleset
            if (!_ruleSets.ContainsKey(DefaultRuleSetName))
            {
                var rules = new List<Rule>();
                var ruleSet = new RuleSet
                {
                    new FileStructureRule(),
                    new OutputPathRule(),
                    new CodeAnalysisRule(),
                };

                _ruleSets.Add(DefaultRuleSetName, ruleSet);
            }

            _fsw.EnableRaisingEvents = true;
        }

        public void LoadRuleSet(Action<IEnumerable<RuleSet>, Exception> onComplete)
        {
            var list = new List<RuleSet>();

            var ruleSet = new RuleSet() { Name = "Default" };

            ruleSet.Add(new FileStructureRule()
            {
                IsEnabled = true,
                Name = "Check File Structure",
                Description = "Check File Structure with template",
                Template = "default.txt"
            });
            ruleSet.Add(new OutputPathRule()
            {
                IsEnabled = true,
                Path = "./output/$(Configuration)/",
                Name = "Check Output Path",
                Description = "Check output path for all project file"
            });

            ruleSet.Add(new CodeAnalysisRule()
            {
                IsEnabled = true,
                Name = "Run Code Analysis",
                Description = "Run code analysis with StyleCop"
            });

            list.Add(ruleSet);

            if (onComplete != null)
            {
                onComplete.Invoke(list, null);
            }
            //throw new NotImplementedException();
        }
    }
}

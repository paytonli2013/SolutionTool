using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Orc.SolutionTool.Model
{
    public class RuleManager : IRuleManager
    {
        private Dictionary<string, List<IRule>> _ruleCache = new Dictionary<string, List<IRule>>();

        public void Persist(IEnumerable<IRule> ruleSet, Action<bool, Exception> onComplete)
        {
            throw new NotImplementedException();
        }

        public void Load(string ruleSet, Action<IEnumerable<IRule>, Exception> onComplete)
        {
            var xe = null as Exception;
            var rules = null as IEnumerable<IRule>;

            if (!_ruleCache.ContainsKey(ruleSet))
            {
                var cd = Environment.CurrentDirectory;
                var dir = System.IO.Path.Combine(cd, @".\Rules\");

                if (!System.IO.Directory.Exists(dir))
                {
                    xe = new Exception("Rules directory does not exists. ");

                    if (onComplete != null)
                    {
                        onComplete(rules, xe);

                        return;
                    }

                    var pattern = ruleSet + ".xml";
                    var file = System.IO.Directory.GetFiles(dir, pattern).FirstOrDefault();

                    if (file == null)
                    {
                        xe = new Exception("Ruleset [" + pattern + "] does not exists. ");

                        if (onComplete != null)
                        {
                            onComplete(rules, xe);

                            return;
                        }
                    }

                    var xdoc = XDocument.Load(file);

                    foreach (var i in xdoc.Root.Elements())
                    {

                    }
                }
            }

            rules = _ruleCache[ruleSet];

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class RuleSet : Orc.SolutionTool.Model.IRuleSet
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        IEnumerable<IRule> rules;

        public IEnumerable<IRule> Rules
        {
            get { return rules; }
            set { rules = value; }
        }
    }
}

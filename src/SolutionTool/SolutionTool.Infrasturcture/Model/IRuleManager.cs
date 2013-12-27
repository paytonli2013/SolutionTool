using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IRuleManager
    {
        void Persist(IEnumerable<IRule> ruleSet, Action<bool, Exception> onComplete);

        void Load(string ruleSet, Action<IEnumerable<IRule>, Exception> onComplete);
    }
}

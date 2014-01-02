using System;
using System.Collections.Generic;

namespace Orc.SolutionTool
{
    public interface IRuleManager
    {
        Dictionary<string, IEnumerable<IRule>> RuleSets { get; }

        void Persist(IEnumerable<IRule> ruleSet, Action<bool, Exception> onComplete);
    }
}

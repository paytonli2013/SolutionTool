using System;
using System.Collections.Generic;

namespace Orc.SolutionTool.Model
{
    public interface IRuleManager
    {
        string DefaultRuleSetName { get; }
        RuleSet DefaultRuleSet { get; }
        Dictionary<string, RuleSet> RuleSets { get; }

        void Persist(RuleSet ruleSet, Action<bool, Exception> onComplete);

        void LoadRuleSet(Action<IEnumerable<RuleSet>, Exception> onComplete);
    }
}

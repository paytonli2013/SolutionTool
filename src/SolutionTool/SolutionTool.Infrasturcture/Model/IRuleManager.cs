using System;
using System.Collections.Generic;
using Orc.SolutionTool.Model.Rules;

namespace Orc.SolutionTool.Model
{
    public interface IRuleManager
    {
        string DefaultRuleSetName { get; }
        RuleSet DefaultRuleSet { get; }
        Dictionary<string, RuleSet> RuleSets { get; }

        void Persist(RuleSet ruleSet, Action<bool, Exception> onComplete);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool
{
    public interface IRuleManager
    {
        void Persist(IRuleSet ruleSet, Action<bool, Exception> onComplete);

        void Load(string ruleSet, Action<IRuleSet, Exception> omComplete);
    }
}

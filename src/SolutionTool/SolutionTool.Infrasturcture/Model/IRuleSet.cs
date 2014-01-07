using System;
using System.Collections.Generic;
namespace Orc.SolutionTool.Model
{
    public interface IRuleSet : IEnumerable<IRule> 
    {
        string Name { get; }
    }
}

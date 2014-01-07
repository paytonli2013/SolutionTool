using System;
using System.Collections.Generic;
namespace Orc.SolutionTool.Model
{
    public interface IRuleSet : IEnumerable<Rule> 
    {
        string Name { get; }
    }
}

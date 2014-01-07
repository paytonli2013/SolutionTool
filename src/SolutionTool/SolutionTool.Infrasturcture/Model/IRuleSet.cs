using System;
namespace Orc.SolutionTool.Model
{
    public interface IRuleSet
    {
        string Name { get; }
        System.Collections.Generic.IEnumerable<IRule> Rules { get; }
    }
}

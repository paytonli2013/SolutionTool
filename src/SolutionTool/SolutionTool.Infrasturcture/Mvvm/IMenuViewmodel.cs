using System;
using Microsoft.Practices.Prism;

namespace Orc.SolutionTool.Mvvm
{
    public interface IMenuViewmodel : IViewModel , IActiveAware
    {
        void Navigate(string region,string view,Action<bool,Exception> onComplete);

        string Title {get;}
        
        bool IsEnable { get; set; }

        bool IsVisible { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Model
{
    public class Directory
    {
        IList<Directory> SubDirectory { get; set; }

        IList<string> Files { get; set; }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Orc.SolutionTool.Model.Rules
{
    public class CheckCsprojOutputPathRule : Rule
    {
        public CheckCsprojOutputPathRule()
        {

        }

        public override void Exam(Context context, Action<ExamResult> action)
        {
            var result = new ExamResult();
            var csprojs = System.IO.Directory.GetFiles(context.Repository.Path, "*.csproj", SearchOption.AllDirectories);
            var ns = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var format = "./output/{0}/.NET{1}/";
            var re = new Regex(@"(?<c>\w+)\|(?<p>\w+)");
            var uri = new Uri(context.Repository.Path);

            foreach (var i in csprojs)
            {
                var fi = new FileInfo(i);
                var doc = XDocument.Load(i);
                var eleAssemblyName = doc.Descendants(ns + "AssemblyName").First().Value;
                var eleTargetFrameworkVersion = doc.Descendants(ns + "TargetFrameworkVersion").First().Value;
                var eleOutputPaths = doc.Descendants(ns + "OutputPath").ToList();

                foreach (var j in eleOutputPaths)
                {
                    var condition = j.Parent.Attribute("Condition").Value;
                    var config = null as string;
                    var platform = null as string;

                    var match = re.Match(condition);

                    if (match.Success)
                    {
                        config = match.Groups["c"].Value;
                        platform = match.Groups["p"].Value;
                    }

                    var formatted = string.Format(format, config,
                        eleTargetFrameworkVersion.Replace("v", "").Replace(".", ""));

                    var uri2Tgt = new Uri(uri, formatted);
                    var uri2Prj = new Uri(fi.Directory.FullName);
                    var uriDiff = uri2Prj.MakeRelativeUri(uri2Tgt);
                    var expected = uriDiff.ToString().Replace('/', '\\');

                    Debug.WriteLine(j.Value + " --> " + expected);
                }
            }

        }

        public override void Apply(Context context, Action<ApplyResult> action)
        {
            throw new NotImplementedException();
        }
    }
}

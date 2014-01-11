using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("ruleSet")]
    public class RuleSet : IRuleSet, IList<Rule>, IXmlSerializable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        private List<Rule> _rules = new List<Rule>();

        public int IndexOf(Rule item)
        {
            return _rules.IndexOf(item);
        }

        public void Insert(int index, Rule item)
        {
            _rules.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _rules.RemoveAt(index);
        }

        public Rule this[int index]
        {
            get
            {
                return _rules[index];
            }
            set
            {
                _rules[index] = value;
            }
        }

        public void Add(Rule item)
        {
            _rules.Add(item);
        }

        public void Clear()
        {
            _rules.Clear();
        }

        public bool Contains(Rule item)
        {
            return _rules.Contains(item);
        }

        public void CopyTo(Rule[] array, int arrayIndex)
        {
            _rules.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _rules.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Rule item)
        {
            return _rules.Remove(item);
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            return _rules.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (!reader.Read())
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var name = reader.Name;
                var xml = reader.ReadOuterXml();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(xml))
                {
                    continue;
                }

                var type = FindType(name);
                var xs = new XmlSerializer(type);

                using (var sr = new System.IO.StringReader(xml))
                {
                    var rule = xs.Deserialize(sr) as Rule;

                    _rules.Add(rule);
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (var i in _rules)
            {
                var type = i.GetType();
                var xs = new XmlSerializer(type);
                var ns = new XmlSerializerNamespaces();

                ns.Add("", "");
                xs.Serialize(writer, i, ns);
            }
        }

        static Type[] _knownTypes = new Type[] { typeof(FileStructureRule), typeof(OutputPathRule), typeof(CodeAnalysisRule), };

        static Type FindType(string name)
        {
            var type = _knownTypes.FirstOrDefault(x =>
            {
                var attrs = x.GetCustomAttributes(false);
                var b = attrs.Any(y =>
                {
                    var attr = y as XmlRootAttribute;

                    return attr != null && attr.ElementName == name;
                });

                return b;
            });

            return type;
        }
    }

    [XmlRoot("rule")]
    public abstract class Rule
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("enabled")]
        public bool IsEnabled { get; set; }

        public Rule()
        {
            IsEnabled = true;
        }

        public abstract void Exam(ExamContext context);
    }

    public class ExamContext
    {
        public Dictionary<string, List<string>> Outputs { get; set; }
        public List<ExamResult> Results { get; set; }
        public List<Violation> Violations { get; set; }
        public Project Project { get; set; }

        public ExamContext(Project project)
        {
            Project = project;
            Outputs = new Dictionary<string, List<string>>();
            Results = new List<ExamResult>();
            Violations = new List<Violation>();
        }

        public void WriteOutput(string ruleName, string output)
        {
            var lines = null as List<string>;

            if (Outputs.ContainsKey(ruleName))
            {
                lines = Outputs[ruleName];
            }
            else
            {
                lines = new List<string>();
                Outputs.Add(ruleName, lines);
            }

            lines.Add(output);
        }

        public void AddResult(ExamResult result)
        {
            Results.Add(result);
        }

        public void AddViolation(Violation violation)
        {
            Violations.Add(violation);
        }

        public Report GenerateReport() 
        {
            var report = new Report()
            {
                Project = Project.Name,
                Items = new List<ReportItem>(),
                CreateAt = DateTime.Now
            };

            foreach (var rule in Project.RuleSet)
            {
                var violations = Violations.Where(v => v.RuleName == rule.Name).ToList();

                var item = new ReportItem
                {
                    Violations = violations,
                    Outputs = Outputs.ContainsKey(rule.Name)?Outputs[rule.Name]:null
                };

                report.Items.Add(item);
            }

            return report;
        }
    }

    [XmlRoot("fileStructure")]
    public class FileStructureRule : Rule
    {
        const string ViolationText = "Specified file should exist but it missing";
        const string ErrorViolationText = "Error occued during rule examination";

        /// <summary>
        /// The template file to use for checking file structure,
        /// which is located at ./Templates/*.xml of the app.
        /// </summary>
        [XmlAttribute("template")]
        public string Template { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as string;

            if (!IsEnabled)
            {
                output = "***" + Name + "***";
                context.WriteOutput(Name, output);

                output = "Scape rule checking since it's not enabled. ";
                context.WriteOutput(Name, output);

                return;
            }

            if (string.IsNullOrWhiteSpace(Template))
            {
                throw new Exception("Template is not specified. ");
            }

            //Don't load template, use the copy

            var tplMgr = new TemplateManager();

            tplMgr.LoadTemplate(Template, (x, y, z) =>
            {
                if (z != null)
                {
                    context.AddResult(new ExamResult { RuleName = Name, Status = ActionStatus.Failed, });
                    context.AddViolation(new Violation { RuleName = Name, Description = ErrorViolationText });

                    output = z.ToString();
                    context.WriteOutput(Name, output);

                    return;
                }

                if (x != null)
                {
                    var outputs = new Dictionary<Directive, List<string>>();

                    x.Execute(context.Project.Path, ref outputs);

                    foreach (var i in outputs)
                    {
                        output = i.Key.Pattern;
                        context.WriteOutput(Name, output);
                        output = new string('-', 80);
                        context.WriteOutput(Name, output);

                        foreach (var j in i.Value)
                        {
                            output = j;
                            context.WriteOutput(Name, output);
                        }
                    }
                }
            });
        }
    }

    [XmlRoot("outputPath")]
    public class OutputPathRule : Rule
    {
        const string PATH_OK = "PO";
        const string PATH_NG = "PN";

        Dictionary<string, List<string>> _dict;

        /// <summary>
        /// The OutputPath node value of a *.csproj file. 
        /// Normally it is set in PropertyGroup section like below: 
        /// <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' " />
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as string;

            if (!IsEnabled)
            {
                output = "***" + Name + "***";
                context.WriteOutput(Name, output);

                output = "Scape rule checking since it's not enabled. ";
                context.WriteOutput(Name, output);

                return;
            }

            _dict = new Dictionary<string, List<string>>();
            _dict.Add(PATH_OK, new List<string>());
            _dict.Add(PATH_NG, new List<string>());

            var root = System.IO.Path.GetFullPath(context.Project.Path);
            var csprojs = System.IO.Directory.GetFiles(
                root, "*.csproj",
                System.IO.SearchOption.AllDirectories
                );
            var ns = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var re = new Regex(@"(?<c>\w+)\|(?<p>\w+)");
            var uri = new Uri(root);

            output = "***" + Name + "***";
            context.WriteOutput(Name, output);

            foreach (var i in csprojs)
            {
                var fi = new System.IO.FileInfo(i);
                var doc = XDocument.Load(i);
                //var eleTargetFrameworkVersion = doc.Descendants(ns + "TargetFrameworkVersion").First().Value;
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

                    var uri2Tgt = new Uri(uri, Path);
                    var uri2Prj = new Uri(fi.FullName);
                    var uri2PrjDir = new Uri(fi.Directory.FullName);
                    var uriDiff = uri2PrjDir.MakeRelativeUri(uri2Tgt);
                    var uriDiff2Root = uri.MakeRelativeUri(uri2Prj);
                    var expected = uriDiff.ToString();
                    var expected2Root = uriDiff2Root.ToString().Replace("/", "\\");
                    var expected2RootIx = expected2Root.IndexOf("\\");
                    var path1 = System.IO.Path.GetFullPath(j.Value);
                    var path2 = System.IO.Path.GetFullPath(expected);

                    output = ".\\" + (expected2RootIx == -1 ? expected2Root : expected2Root.Substring(expected2RootIx + 1));

                    if (string.Compare(path1, path2, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        _dict[PATH_OK].Add(output);
                        output = j.Value + " <--> " + expected;
                        _dict[PATH_OK].Add(output);
                    }
                    else
                    {
                        _dict[PATH_NG].Add(output);
                        output = j.Value + " ---> " + expected;
                        _dict[PATH_NG].Add(output);
                    }
                }
            }

            if (_dict[PATH_OK].Count > 0)
            {
                output = new string('-', 80);
                context.WriteOutput(Name, output);

                output = "Path OK";
                context.WriteOutput(Name, output);

                foreach (var i in _dict[PATH_OK])
                {
                    output = i;
                    context.WriteOutput(Name, output);
                }
            }

            if (_dict[PATH_NG].Count > 0)
            {
                output = new string('-', 80);
                context.WriteOutput(Name, output);

                output = "Path NG";
                context.WriteOutput(Name, output);

                foreach (var i in _dict[PATH_NG])
                {
                    output = i;
                    context.WriteOutput(Name, output);
                }
            }

        }
    }

    [XmlRoot("codeAnalysis")]
    public class CodeAnalysisRule : Rule
    {
        /// <summary>
        /// Path to InpsectCode, e.g, d:\Program Files\Tools\InspectCode.exe. 
        /// If not specified, this app will try to resolve it from system registry. 
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as string;

            if (!IsEnabled)
            {
                output = "***" + Name + "***";
                context.WriteOutput(Name, output);

                output = "Scape rule checking since it's not enabled. ";
                context.WriteOutput(Name, output);

                return;
            }

            if (string.IsNullOrWhiteSpace(Path))
            {
                output = "Need to specify InspectCode executable path. ";
                context.WriteOutput(Name, output);

                return;
            }

            if (!System.IO.File.Exists(Path))
            {
                output = "Cannot find InspectCode app at [" + Path + "]. ";
                context.WriteOutput(Name, output);

                return;
            }

            output = "***" + Name + "***";
            context.WriteOutput(Name, output);

            var root = context.Project.Path;
            var uri = new Uri(root);
            var rptFi = new System.IO.FileInfo(System.IO.Path.Combine(root, "Reports"));

            if (!System.IO.Directory.Exists(rptFi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(rptFi.Directory.FullName);

                return;
            }

            var cachesHome = System.IO.Path.Combine(rptFi.Directory.FullName, "cache");
            var slns = System.IO.Directory.GetFiles(root, "*.sln", System.IO.SearchOption.AllDirectories);

            foreach (var sln in slns)
            {
                var uriSln = new Uri(sln);
                var uri2Root = uri.MakeRelativeUri(uriSln).ToString().Replace("/", "\\");
                var uri2RootIx = uri2Root.IndexOf('\\');
                var rpt = string.Format(@".\Reports\InspectCode_Report_{0:yyyyMMddHHmmssfff}.xml", DateTime.Now);
                var cmdLine = string.Format(@"/o=""{1}"" ""{2}""",
                    cachesHome, rpt, sln);

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path,
                        Arguments = cmdLine,
                        WorkingDirectory = root,
                        //CreateNoWindow = true,
                        //WindowStyle = ProcessWindowStyle.Hidden,
                        //RedirectStandardInput = true,
                        //RedirectStandardError = true,
                        //RedirectStandardOutput = true,
                        //UseShellExecute = false,
                        ErrorDialog = true,
                    },
                };

                proc.Start();
                proc.WaitForExit();

                output = ".\\" + (uri2RootIx == -1 ? uri2Root : uri2Root.Substring(uri2RootIx + 1)) + ": " + rpt;
                context.WriteOutput(Name, output);
            }
        }
    }
}

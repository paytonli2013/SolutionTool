using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

                using (var sr = new StringReader(xml))
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
        public Dictionary<string, List<Output>> Outputs { get; set; }
        public List<ExamResult> Results { get; set; }
        public List<Violation> Violations { get; set; }
        public Project Project { get; set; }

        public ExamContext(Project project)
        {
            Project = project;
            Outputs = new Dictionary<string, List<Output>>();
            Results = new List<ExamResult>();
            Violations = new List<Violation>();
        }

        public void WriteOutput(string ruleName, Output output)
        {
            var outputs = null as List<Output>;

            if (Outputs.ContainsKey(ruleName))
            {
                outputs = Outputs[ruleName];
            }
            else
            {
                outputs = new List<Output>();
                Outputs.Add(ruleName, outputs);
            }

            outputs.Add(output);
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
                var outputs = Outputs.ContainsKey(rule.Name) ? Outputs[rule.Name] : null;

                var item = new ReportItem
                {
                    Name = rule.Name,
                    Description = rule.Description,
                    Violations = violations,
                    Outputs = outputs,
                };

                report.Items.Add(item);
            }

            return report;
        }
    }

    [XmlRoot("fileStructure")]
    public class FileStructureRule : Rule
    {
        /// <summary>
        /// The template file to use for checking file structure,
        /// which is located at ./Templates/*.xml of the app.
        /// </summary>
        [XmlAttribute("template")]
        public string Template { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as Output;

            if (!IsEnabled)
            {
                output = new Output { Status = Output.STATUS_FAILED, Summary = "Rule [" + Name + "] is disabled. ", };
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
                    context.AddResult(new ExamResult
                    {
                        RuleName = Name,
                        Status = ActionStatus.Failed,
                    });
                    context.AddViolation(new Violation
                    {
                        RuleName = Name,
                        Description = "Can not load file structure template file. ",
                    });

                    output = new Output
                    {
                        Status = Output.STATUS_FAILED,
                        Summary = z.Message,
                        Details = new List<string>
                        {
                            z.ToString(),
                        }
                    };
                    context.WriteOutput(Name, output);

                    return;
                }

                if (x != null)
                {
                    var outputs = new Dictionary<Directive, List<string>>();

                    x.Execute(context.Project.Path, ref outputs);

                    foreach (var i in outputs)
                    {
                        output = new Output
                        {
                            Summary = i.Key.Pattern,
                            Details = new List<string>(),
                        };

                        foreach (var j in i.Value)
                        {
                            output.Details.Add(j);
                        }

                        if (i.Value.Count == 0)
                        {
                            output.Status = Output.STATUS_PASS;
                            output.Details.Add("Pass");
                        }
                        else if (i.Key is FolderDirective || i.Key is FileDirective)
                        {
                            output.Status = Output.STATUS_FAILED;
                        }

                        context.WriteOutput(Name, output);
                    }
                }
            });
        }
    }

    [XmlRoot("outputPath")]
    public class OutputPathRule : Rule
    {
        /// <summary>
        /// The OutputPath node value of a *.csproj file. 
        /// Normally it is set in PropertyGroup section like below: 
        /// <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' " />
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as Output;

            if (!IsEnabled)
            {
                output = new Output { Status = Output.STATUS_WARNING, Summary = "Rule [" + Name + "] is disabled. ", };
                context.WriteOutput(Name, output);

                return;
            }

            var root = System.IO.Path.GetFullPath(context.Project.Path);
            var csprojs = Directory.GetFiles(
                root, "*.csproj",
                SearchOption.AllDirectories
                );
            var ns = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var re = new Regex(@"(?<c>\w+)\|(?<p>\w+)");
            var uri = new Uri(root);

            foreach (var i in csprojs)
            {
                var fi = new FileInfo(i);
                var doc = XDocument.Load(i);
                //var eleTargetFrameworkVersion = doc.Descendants(ns + "TargetFrameworkVersion").First().Value;
                var eleOutputPaths = doc.Descendants(ns + "OutputPath").ToList();
                var uri2Tgt = new Uri(uri, Path);
                var uri2Prj = new Uri(fi.FullName);
                var uri2PrjDir = new Uri(fi.Directory.FullName);
                var uriDiff = uri2PrjDir.MakeRelativeUri(uri2Tgt);
                var uriDiff2Root = uri.MakeRelativeUri(uri2Prj);
                var expected = uriDiff.ToString();
                var expected2Root = uriDiff2Root.ToString().Replace("/", "\\");
                var expected2RootIx = expected2Root.IndexOf("\\");
                var pathCsproj = ".\\" + (expected2RootIx == -1
                    ? expected2Root : expected2Root.Substring(expected2RootIx + 1));

                output = new Output
                {
                    Summary = pathCsproj,
                    Details = new List<string>(),
                };

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

                    var path1 = System.IO.Path.GetFullPath(j.Value);
                    var path2 = System.IO.Path.GetFullPath(expected);

                    if (string.Compare(path1, path2, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        output.Status = Output.STATUS_PASS;
                        output.Details.Add(j.Value + " <--> " + expected);
                    }
                    else
                    {
                        output.Status = Output.STATUS_FAILED;
                        output.Details.Add(j.Value + " ---> " + expected);
                    }
                }

                context.WriteOutput(Name, output);
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
            var output = null as Output;

            if (!IsEnabled)
            {
                output = new Output
                {
                    Status = Output.STATUS_WARNING,
                    Summary = "Rule [" + Name + "] is disabled. ",
                };
                context.WriteOutput(Name, output);

                return;
            }

            if (string.IsNullOrWhiteSpace(Path))
            {
                output = new Output
                {
                    Status = Output.STATUS_WARNING,
                    Summary = "Need to specify InspectCode executable path. ",
                };
                context.WriteOutput(Name, output);

                return;
            }

            if (!File.Exists(Path))
            {
                output = new Output
                {
                    Status = Output.STATUS_WARNING,
                    Summary = "Cannot find InspectCode app at [" + Path + "]. ",
                };
                context.WriteOutput(Name, output);

                return;
            }

            var rptDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            var rptName = string.Format(@"InspectCode_Report_{0:yyyyMMddHHmmssfff}.xml", DateTime.Now);            
            var rpt = @".\Reports\" + rptName;
            var cachesHome = @".\Reports\cache";
            var rptFullName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rpt);

            if (!Directory.Exists(rptDir))
            {
                Directory.CreateDirectory(rptDir);

                return;
            }

            var slns = Directory.GetFiles(context.Project.Path, "*.sln", SearchOption.AllDirectories);

            foreach (var sln in slns)
            {
                var uriSln = new Uri(sln);
                var cmdLine = string.Format(@"/o=""{1}"" ""{2}""",
                    cachesHome, rpt, sln);

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path,
                        Arguments = cmdLine,
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
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

                output = new Output 
                {
                    Summary = "InspectCode", 
                    Details = new List<string> { rptFullName, },
                };                
                context.WriteOutput(Name, output);
            }
        }
    }
}

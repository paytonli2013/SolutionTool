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

        public Project Project { get; set; }

        public ExamContext(Project project)
        {
            Project = project;
            Outputs = new Dictionary<string, List<string>>();
            Results = new List<ExamResult>();
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
    }

    [XmlRoot("fileStructure")]
    public class FileStructureRule : Rule
    {
        const string DIR_EXISTS = "DE";
        const string DIR_MISSING = "DM";
        const string FILE_EXISTS = "FE";
        const string FILE_MISSING = "FM";

        Stack<String> _dir2Root;
        Dictionary<string, List<string>> _dict;

        [XmlAttribute("template")]
        public string Template { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as string;

            if (!IsEnabled)
            {
                output = "***" + Name + "***";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

                output = "Scape rule checking since it's not enabled. ";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

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

                    output = z.ToString();
                    context.WriteOutput(Name, output);
                    Debug.WriteLine(output);

                    return;
                }

                if (x != null)
                {
                    _dir2Root = new Stack<string>();

                    _dict = new Dictionary<string, List<string>>();
                    _dict.Add(DIR_EXISTS, new List<string>());
                    _dict.Add(DIR_MISSING, new List<string>());
                    _dict.Add(FILE_EXISTS, new List<string>());
                    _dict.Add(FILE_MISSING, new List<string>());

                    InnerExam(context, x);

                    output = "***" + Name + "***";
                    context.WriteOutput(Name, output);
                    Debug.WriteLine(output);

                    if (_dict[DIR_EXISTS].Count > 0)
                    {
                        output = new string('-', 80);
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        output = "Directory Exists";
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        foreach (var i in _dict[DIR_EXISTS])
                        {
                            output = i;
                            context.WriteOutput(Name, output);
                            Debug.WriteLine(output);
                        }
                    }

                    if (_dict[DIR_MISSING].Count > 0)
                    {
                        output = new string('-', 80);
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        output = "Directory Missing";
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        foreach (var i in _dict[DIR_MISSING])
                        {
                            output = i;
                            context.WriteOutput(Name, output);
                            Debug.WriteLine(output);
                        }
                    }

                    if (_dict[FILE_EXISTS].Count > 0)
                    {
                        output = new string('-', 80);
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        output = "File Exists";
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        foreach (var i in _dict[FILE_EXISTS])
                        {
                            output = i;
                            context.WriteOutput(Name, output);
                            Debug.WriteLine(output);
                        }
                    }

                    if (_dict[FILE_MISSING].Count > 0)
                    {
                        output = new string('-', 80);
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        output = "File Missing";
                        context.WriteOutput(Name, output);
                        Debug.WriteLine(output);

                        foreach (var i in _dict[FILE_MISSING])
                        {
                            output = i;
                            context.WriteOutput(Name, output);
                            Debug.WriteLine(output);
                        }
                    }

                    if (_dict[DIR_MISSING].Count > 0 || _dict[FILE_MISSING].Count > 0)
                    {
                        context.AddResult(new ExamResult { RuleName = Name, Status = ActionStatus.Failed, });
                    }
                    else
                    {
                        context.AddResult(new ExamResult { RuleName = Name, Status = ActionStatus.Pass, });
                    }
                }
            });
        }

        private void InnerExam(ExamContext context, Directory dir)
        {
            _dir2Root.Push(dir.Name);

            var rel = string.Empty;

            foreach (var i in _dir2Root.Reverse())
            {
                rel = System.IO.Path.Combine(rel, i);
            }

            var root = context.Project.Path;
            var full = System.IO.Path.Combine(root, rel);

            if ((dir.Ocurr ?? 1) >= 1 && !System.IO.Directory.Exists(full))
            {
                _dict[DIR_MISSING].Add(rel);
            }
            else
            {
                if ((dir.Ocurr ?? 1) < 1)
                {
                    var fsDirInfo = new System.IO.DirectoryInfo(full).Parent;

                    InnerExamFsDirectory(context, dir, fsDirInfo);
                }
            }

            if (dir.SubDirectories != null)
            {
                foreach (var i in dir.SubDirectories)
                {
                    InnerExam(context, i);
                }
            }

            if (dir.Files != null)
            {
                foreach (var i in dir.Files)
                {
                    var path = System.IO.Path.Combine(full, i.Name);
                    var path2 = System.IO.Path.Combine(rel, i.Name);

                    if (!System.IO.File.Exists(path))
                    {
                        _dict[FILE_MISSING].Add(path2);
                    }
                    //else
                    //{
                    //    _dict[FILE_EXISTS].Add(path2);
                    //}
                }
            }

            _dir2Root.Pop();
        }

        private void InnerExamFsDirectory(ExamContext context, Directory dir, System.IO.DirectoryInfo fsDirInfo)
        {
            if (!fsDirInfo.Exists)
            {
                return;
            }

            var fsDirs = fsDirInfo.GetDirectories();

            foreach (var i in fsDirs)
            {
                var eq = string.Compare(dir.Name, i.Name, StringComparison.OrdinalIgnoreCase) == 0;
                var uri = new Uri(i.FullName);
                var uriRoot = new Uri(context.Project.Path);
                var uriDiff2Root = uriRoot.MakeRelativeUri(uri);
                var rel2Root = uriDiff2Root.ToString().Replace("/", "\\");
                var rel2RootIx = rel2Root.IndexOf("\\");

                if (eq && (dir.Ocurr ?? 1) < 1 && System.IO.Directory.Exists(i.FullName))
                {
                    _dict[DIR_EXISTS].Add(".\\" + (rel2RootIx == -1 ? rel2Root : rel2Root.Substring(rel2RootIx + 1)));
                }

                if ((dir.Recursive ?? false) && System.IO.Directory.Exists(i.FullName))
                {
                    InnerExamFsDirectory(context, dir, i);
                }
            }
        }
    }

    [XmlRoot("outputPath")]
    public class OutputPathRule : Rule
    {
        const string PATH_OK = "PO";
        const string PATH_NG = "PN";

        Dictionary<string, List<string>> _dict;

        [XmlAttribute("path")]
        public string path { get; set; }

        public override void Exam(ExamContext context)
        {
            var output = null as string;

            if (!IsEnabled)
            {
                output = "***" + Name + "***";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

                output = "Scape rule checking since it's not enabled. ";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

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
            Debug.WriteLine(output);

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

                    var uri2Tgt = new Uri(uri, path);
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
                Debug.WriteLine(output);

                output = "Path OK";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

                foreach (var i in _dict[PATH_OK])
                {
                    output = i;
                    context.WriteOutput(Name, output);
                    Debug.WriteLine(output);
                }
            }

            if (_dict[PATH_NG].Count > 0)
            {
                output = new string('-', 80);
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

                output = "Path NG";
                context.WriteOutput(Name, output);
                Debug.WriteLine(output);

                foreach (var i in _dict[PATH_NG])
                {
                    output = i;
                    context.WriteOutput(Name, output);
                    Debug.WriteLine(output);
                }
            }

        }
    }

    [XmlRoot("codeAnalysis")]
    public class CodeAnalysisRule : Rule
    {
        public override void Exam(ExamContext context)
        {
        }
    }
}

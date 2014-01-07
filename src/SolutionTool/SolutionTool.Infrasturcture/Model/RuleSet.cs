using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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
            if (string.IsNullOrWhiteSpace(Template))
            {
                throw new Exception("Template is not specified. ");
            }

            var tplMgr = new TemplateManager();

            tplMgr.LoadTemplate(Template, (x, y, z) => 
            {
                if (z != null)
                {
                    context.AddResult(new ExamResult { RuleName = Name, Status = ActionStatus.Failed, });
                    context.WriteOutput(Name, y.ToString());

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

                    context.WriteOutput(Name, "***" + Name + "***");

                    if (_dict[DIR_EXISTS].Count > 0)
                    {
                        context.WriteOutput(Name, new string('-', 80));
                        context.WriteOutput(Name, "Directory Exists");

                        foreach (var i in _dict[DIR_EXISTS])
                        {
                            context.WriteOutput(Name, i);
                        }
                    }

                    if (_dict[DIR_MISSING].Count > 0)
                    {
                        context.WriteOutput(Name, new string('-', 80));
                        context.WriteOutput(Name, "Directory Missing");

                        foreach (var i in _dict[DIR_MISSING])
                        {
                            context.WriteOutput(Name, i);
                        }
                    }

                    if (_dict[FILE_EXISTS].Count > 0)
                    {
                        context.WriteOutput(Name, new string('-', 80));
                        context.WriteOutput(Name, "File Exists");

                        foreach (var i in _dict[FILE_EXISTS])
                        {
                            context.WriteOutput(Name, i);
                        }
                    }

                    if (_dict[FILE_MISSING].Count > 0)
                    {
                        context.WriteOutput(Name, new string('-', 80));
                        context.WriteOutput(Name, "File Missing");

                        foreach (var i in _dict[FILE_MISSING])
                        {
                            context.WriteOutput(Name, i);
                        }
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

            var full = System.IO.Path.Combine(context.Project.Path, rel);

            if (!System.IO.Directory.Exists(full))
            {
                _dict[DIR_MISSING].Add(rel);
            }
            else
            {
                _dict[DIR_EXISTS].Add(rel);
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

                    if (!System.IO.Directory.Exists(path))
                    {
                        _dict[FILE_MISSING].Add(path2);
                    }
                    else
                    {
                        _dict[FILE_EXISTS].Add(path2);
                    }
                }
            }

            _dir2Root.Pop();
        }
    }

    [XmlRoot("outputPath")]
    public class OutputPathRule : Rule
    {
        [XmlAttribute("path")]
        public string path { get; set; }

        public override void Exam(ExamContext context)
        {
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

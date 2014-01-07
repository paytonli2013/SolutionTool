using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.ServiceLocation;

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
        Dictionary<string, List<string>> Outputs { get; set; }
        List<ExamResult> Results { get; set; }

        public void WriteOutput(string ruleName, string output)
        {

        }

        public void AddResult(ExamResult result)
        {

        }
    }

    [XmlRoot("fileStructure")]
    public class FileStructureRule : Rule
    {
        [XmlAttribute("template")]
        public string Template { get; set; }

        public override void Exam(ExamContext context)
        {
            if (string.IsNullOrWhiteSpace(Template))
            {
                throw new Exception("Template is not specified. ");
            }

            var tplMgr = ServiceLocator.Current.GetInstance<ITemplateManager>();
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

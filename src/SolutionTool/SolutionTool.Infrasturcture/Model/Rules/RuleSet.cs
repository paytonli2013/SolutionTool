using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Orc.SolutionTool.Model.Rules
{
    [XmlRoot("ruleSet")]
    public class RuleSet : IList<XRule>, IXmlSerializable,IRuleSet
    {
        private List<XRule> _rules = new List<XRule>();

        public int IndexOf(XRule item)
        {
            return _rules.IndexOf(item);
        }

        public void Insert(int index, XRule item)
        {
            _rules.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _rules.RemoveAt(index);
        }

        public XRule this[int index]
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

        public void Add(XRule item)
        {
            _rules.Add(item);
        }

        public void Clear()
        {
            _rules.Clear();
        }

        public bool Contains(XRule item)
        {
            return _rules.Contains(item);
        }

        public void CopyTo(XRule[] array, int arrayIndex)
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

        public bool Remove(XRule item)
        {
            return _rules.Remove(item);
        }

        public IEnumerator<XRule> GetEnumerator()
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
                    var rule = xs.Deserialize(sr) as XRule;

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

        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        IEnumerator<IRule> IEnumerable<IRule>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [XmlRoot("rule")]
    public abstract class XRule : IRule
    {
        [XmlAttribute("enabled")]
        public bool IsEnabled { get; set; }

        public XRule()
        {
            IsEnabled = true;
        }

        public abstract void Exam(ExamContext context, Action<ExamResult> onComplete);

        public abstract void Apply(ExamContext context, Action<ExamResult> onComplete);

        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("desc")]
        public string Description
        {
            get;
            set;
        }
    }

    [XmlRoot("fileStructure")]
    public class FileStructureRule : XRule
    {
        [XmlAttribute("template")]
        public string Template { get; set; }

        public override void Exam(ExamContext context, Action<ExamResult> onComplete)
        {
            //throw new NotImplementedException();
        }

        public override void Apply(ExamContext context, Action<ExamResult> onComplete)
        {
            //throw new NotImplementedException();
        }
    }

    [XmlRoot("outputPath")]
    public class OutputPathRule : XRule
    {
        [XmlAttribute("path")]
        public string path { get; set; }

        public override void Exam(ExamContext context, Action<ExamResult> onComplete)
        {
            //throw new NotImplementedException();
        }

        public override void Apply(ExamContext context, Action<ExamResult> onComplete)
        {
            throw new NotImplementedException();
        }
    }

    [XmlRoot("codeAnalysis")]
    public class CodeAnalysisRule : XRule
    {

        public override void Exam(ExamContext context, Action<ExamResult> onComplete)
        {
            //throw new NotImplementedException();
        }

        public override void Apply(ExamContext context, Action<ExamResult> onComplete)
        {
            //throw new NotImplementedException();
        }
    }
}

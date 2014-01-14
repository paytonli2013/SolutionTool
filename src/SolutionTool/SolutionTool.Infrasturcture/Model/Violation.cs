using System.Xml.Serialization;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("violation")]
    public class Violation
    {
        [XmlAttribute("rulename")]
        public string RuleName { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}

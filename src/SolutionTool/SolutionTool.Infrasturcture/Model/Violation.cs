using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

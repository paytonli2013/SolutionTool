using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("report")]
    public class Report
    {
        #region Properties

        [XmlAttribute("project")]
        public string Project { get; set; }

        [XmlAttribute("summary")]
        public string Summary { get; set; }

        [XmlAttribute("createTime")]
        public DateTime CreateAt { get; set; }

        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<ReportItem> Items { get; set; }

        #endregion

        static string templatePath = Environment.CurrentDirectory + "\\Templates\\RptHtml.xslt";
        static string templatePathPlain = Environment.CurrentDirectory + "\\Templates\\RptTxt.xslt";

        public static string GetTextFile(string xmlFile)
        {
            if (string.IsNullOrEmpty(xmlFile))
                return "";
            // Generating a temporary HTML file
            string outfile = System.IO.Path.GetTempFileName().Replace(".tmp", ".html");

            //string path = Environment.CurrentDirectory + "\\Logs\\log.xml";
            // Creating the XslCompiledTransform object
            XslCompiledTransform transform = new XslCompiledTransform();

            // Loading the stylesheet file from the textbox
            transform.Load(templatePathPlain);
            transform.Transform(xmlFile, outfile);

            return outfile;
        }

        public static string GetHtmlFile(string xmlFile)
        {
            if (string.IsNullOrEmpty(xmlFile))
                return "";
            // Generating a temporary HTML file
            string outfile = System.IO.Path.GetTempFileName().Replace(".tmp", ".html");

            // Creating the XslCompiledTransform object
            XslCompiledTransform transform = new XslCompiledTransform();

            // Loading the stylesheet file from the textbox
            transform.Load(templatePath);
            transform.Transform(xmlFile, outfile);

            return outfile;
        }

        public ActionStatus Status
        {
            get;
            set;
        }

        public string GetText()
        {
            var sb = new StringBuilder();

            foreach (var i in Items)
            {
                foreach (var j in i.Outputs)
                {
                    sb.AppendLine((j.Summary));

                    if (j.Details != null)
                    {
                        foreach (var k in j.Details)
                        {
                            sb.AppendLine((k));
                        }
                    }
                }
            }

            return sb.ToString();
        }
    }

    [XmlRoot("reportitem")]
    public class ReportItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlElement("violations")]
        public List<Violation> Violations { get; set; }

        [XmlArray("outputs")]
        [XmlArrayItem("output")]
        public List<Output> Outputs { get; set; }
    }

    [XmlRoot("output")]
    public class Output
    {
        [XmlElement("summary")]
        public string Summary { get; set; }
        [XmlArray("details")]
        [XmlArrayItem("detail")]
        public List<string> Details { get; set; }
    }
}

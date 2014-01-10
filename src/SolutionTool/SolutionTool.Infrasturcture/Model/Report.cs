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

        [XmlElement("items")]
        public List<ReportItem> Items { get; set; }

        #endregion

        static string templatePath = Environment.CurrentDirectory + "\\Templates\\ReportTemplate.xslt";
        static string templatePathPlain = Environment.CurrentDirectory + "\\Templates\\ReportTemplate.Plain.xslt";

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

            foreach (var item in Items)
            {
                foreach (var str in item.Outputs)
                {
                    sb.AppendLine((str));
                }
            }
            return sb.ToString();
            //throw new NotImplementedException();
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

        [XmlElement("output")]
        public List<string> Outputs { get; set; }
    }
}

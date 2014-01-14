using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public class RuleRunner : IRuleRunner
    {
        private static readonly string _dirLogs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Logs\");
        private static readonly string _dirReports = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Reports\");

        public void LoadRunLog(Action<IEnumerable<RunLogItem>, Exception> onComplete)
        {
            var list = BuildLogs();

            if (onComplete != null)
                onComplete.Invoke(list, null);
        }

        public void ClearLog(Action<Exception> onComplete)
        {
            var xe = null as Exception;

            if (!Directory.Exists(_dirLogs))
            {
                return;
            }

            if (!Directory.Exists(_dirReports))
            {
                return;
            }

            foreach (var i in Directory.GetFiles(_dirLogs).Concat(Directory.GetFiles(_dirReports)))
            {
                try
                {
                    File.Delete(i);
                }
                catch (Exception x)
                {
                    xe = x;
                }
            }

            if (onComplete != null)
            {
                onComplete(xe);
            }
        }


        List<RunLogItem> list = new List<RunLogItem>();

        private List<RunLogItem> BuildLogs()
        {
            if (!Directory.Exists(_dirLogs))
            {
                return list;
            }

            list.Clear();

            foreach (var i in Directory.GetFiles(_dirLogs, "*.xml"))
            {
                var xdoc = XDocument.Load(i);

                if (!xdoc.Elements().Any())
                {
                    continue;
                }

                var root = xdoc.Elements().FirstOrDefault();

                foreach (var run in root.Elements())
                {
                    if (run.Name != "run")
                    {
                        continue;
                    }

                    var attrProject = run.Attribute("project");
                    var attrRules = run.Attribute("rules");
                    var attrStart = run.Attribute("start");
                    var attrEnd = run.Attribute("end");
                    var attrResult = run.Attribute("status");
                    var attrReport = run.Attribute("report");
                    var status = ActionStatus.None;

                    if (attrResult != null)
                    {
                        Enum.TryParse<ActionStatus>(attrResult.Value, out status);
                    }

                    var prj = new RunLogItem
                    {
                        Project = attrProject == null ? null : attrProject.Value,
                        Rules = attrRules == null ? null : (int?)int.Parse(attrRules.Value),
                        Start = attrStart == null ? null : (DateTime?)DateTime.Parse(attrStart.Value),
                        End = attrEnd == null ? null : (DateTime?)DateTime.Parse(attrEnd.Value),
                        Status = status,
                        Report = attrReport == null ? null : attrReport.Value,
                    };

                    list.Add(prj);
                }
            }

            var rv = list.OrderByDescending(x => x.Start).ToList();

            return rv;
        }

        public event EventHandler<RunLogEventArgs> RunLogAdded;

        public void Exam(Project project, Action<ExamResult> onComplete)
        {
        }

        void FireRunLogAdded(RunLogItem item)
        {


            if (RunLogAdded != null)
            {
                RunLogAdded.Invoke(this, new RunLogEventArgs(item));
            }
        }

        public void RunProject(Project project, Action<Report, RunLogItem> onComplete)
        {
            var log = new RunLogItem();

            log.Start = DateTime.Now;
            log.Project = project.Name;

            if (project == null)
            {
                throw new ArgumentException();
            }

            if (project.RuleSet == null)
            {
                throw new Exception("RuleSet is not specified. ");
            }

            var context = new ExamContext(project);

            foreach (var i in project.RuleSet)
            {
                try
                {
                    i.Exam(context);
                }
                catch (Exception e)
                {
                    context.WriteOutput(i.Name, new Output
                    {
                        Summary = e.Message,
                        Details = new List<string>
                        {
                            e.ToString(),
                        },
                    });
                    context.AddResult(new ExamResult()
                    {
                        RuleName = i.Name,
                        Status = ActionStatus.Failed,
                    });
                    context.AddViolation(new Violation
                    {
                        RuleName = i.Name,
                        Description = e.Message,
                    });
                    continue;
                }
            }

            var report = context.GenerateReport();

            var path = SaveReportToFile(report);

            log.End = DateTime.Now;
            log.Status = report.Status;
            log.Report = path;
            log.Rules = project.RuleSet.Count;

            list.Insert(0, log);

            SaveRunlog(log);

            if (onComplete != null)
            {
                onComplete.Invoke(report, log);
            }
        }

        private void SaveRunlog(RunLogItem log)
        {
            FireRunLogAdded(log);

            if (!Directory.Exists(_dirLogs))
            {
                Directory.CreateDirectory(_dirLogs);
            }

            var path = Path.Combine(_dirLogs, log.Project + ".xml");
            var xdoc = null as XDocument;

            if (File.Exists(path))
            {
                xdoc = XDocument.Load(path);
            }
            else
            {
                xdoc = new XDocument();
            }

            if (xdoc.Root == null)
            {
                xdoc.Add(new XElement("runlog"));
            }

            xdoc.Root.Add(new XElement(
                "run",
                new XAttribute("project", log.Project),
                new XAttribute("rules", log.Rules),
                new XAttribute("start", log.Start),
                new XAttribute("end", log.End),
                new XAttribute("status", log.Status),
                new XAttribute("summary", log.Summary),
                new XAttribute("report", log.Report)
                ));
            xdoc.Save(path);
        }

        private string SaveReportToFile(Report report)
        {
            try
            {
                if (!Directory.Exists(_dirReports))
                {
                    Directory.CreateDirectory(_dirReports);
                }

                var path = string.Format("{0}\\{1}_{2:yyyyMMddHHmmss}.xml",
                    _dirReports, report.Project, DateTime.Now);

                using (var fs = XmlWriter.Create(path))
                {
                    var xs = new XmlSerializer(typeof(Report));

                    xs.Serialize(fs, report);
                }

                return path;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when saving report to file", ex);
            }
        }
    }
}

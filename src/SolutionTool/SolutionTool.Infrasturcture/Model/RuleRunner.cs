using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orc.SolutionTool.Model;
using System.Xml;
using System.Xml.Serialization;

namespace Orc.SolutionTool
{
    public class RuleRunner : IRuleRunner
    {
        const string ErrorViolationText = "Error occued during rule examination";

        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Logs\");

        private static readonly string _dirR = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Report\");

        public void LoadRunLog(Action<IEnumerable<RunLogItem>, Exception> onComplete)
        {
            var list = BuildLogs();

            if (onComplete != null)
                onComplete.Invoke(list, null);
        }

        public void ClearLog(Action<Exception> onComplete)
        {
            var xe = null as Exception;

            if (!System.IO.Directory.Exists(_dir))
            {
                return;
            }

            foreach (var i in System.IO.Directory.GetFiles(_dir))
            {
                try
                {
                    System.IO.File.Delete(i);
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
            if (!System.IO.Directory.Exists(_dir))
            {
                return list;
            }

            foreach (var i in System.IO.Directory.GetFiles(_dir, "*.xml"))
            {
                var xdoc = XDocument.Load(i);

                if (!xdoc.Elements().Any())
                {
                    continue;
                }

                var root = xdoc.Elements().FirstOrDefault();

                foreach (var run in root.Elements())
                {
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

            return list;
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

        public void RunProject(Project project, Action<Report,RunLogItem> onComplete)
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
                    context.WriteOutput(i.Name, string.Format("error occured:\n{0}", e.ToString()));
                    context.AddResult(new ExamResult() { RuleName = i.Name, Status = ActionStatus.Failed });
                    context.AddViolation(new Violation { RuleName = i.Name, Description = ErrorViolationText });
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
            //throw new NotImplementedException();
            if (onComplete != null)
            {
                onComplete.Invoke(report, log);
            }
        }

        private void SaveRunlog(RunLogItem log)
        {
            FireRunLogAdded(log);
        }

        private string SaveReportToFile(Report report)
        {
            try
            {
                if (!System.IO.Directory.Exists(_dirR))
                {
                    System.IO.Directory.CreateDirectory(_dirR);
                }

                var reportPath = string.Format("{0}\\{1}_{2:yyyyMMddHHmmss}.xml", _dirR, report.Project, report.CreateAt);

                using (var fs = XmlWriter.Create(reportPath))
                {
                    var xs = new XmlSerializer(typeof(Report));

                    xs.Serialize(fs, report);
                }

                return reportPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when saving report to file", ex);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    public class RuleRunner : IRuleRunner
    {
        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Logs\");

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

        private static List<RunLogItem> BuildLogs()
        {
            List<RunLogItem> list = new List<RunLogItem>();

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

        public void RunProject(Project project, Action<Report> onComplete)
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
                    continue;
                }
            }

            var report = new Report(context,log);

            log.End = DateTime.Now;
            log.Status = report.Status;
            log.Report = report.GetText();
            log.Rules = project.RuleSet.Count;

            //throw new NotImplementedException();
            if (onComplete != null)
            {
                onComplete.Invoke(report);
            }
        }
    }
}

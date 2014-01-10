using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CommandLine;
using Orc.SolutionTool.Model;

namespace Orc.SolutionTool
{
    /// <summary>
    /// Solution Checker
    /// </summary>
    class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <example>
        /// -r "f:\_E_\SolutionTool" -t default.xml -b "./output/$(Configuration)/" -i "f:\_E_\jb-cl\inspectcode.exe" -l slntool.log
        /// </example>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());

            var options = new Options();
            var parser = new Parser();

            if (!Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            Console.WriteLine("Begin checking " + options.Repository);

            var path = System.IO.Path.GetFullPath(options.Repository);

            if (!System.IO.Directory.Exists(path))
            {
                Console.WriteLine("Repository directory does not exist: [" + options.Repository + "]");
            }
            else
            {
                var di = new DirectoryInfo(path);
                var project = new Project { Name = di.Name, Path = path, CreateTime = DateTime.Now, };
                var ruleSet = new RuleSet 
                {
                    new FileStructureRule
                    { 
                        Name = "File Structure Rule",
                        IsEnabled = true, 
                        Template = options.Template,
                    }, 
                    new OutputPathRule 
                    {
                        Name = "Output Path Rule",
                        IsEnabled = true, 
                        Path = options.OutputBuildPath,
                    },
                };

                if (!string.IsNullOrWhiteSpace(options.InspectCodeExePath))
                {
                    ruleSet.Add(new CodeAnalysisRule
                    {
                        Name = "Code Analysis Rule",
                        IsEnabled = true,
                        Path = options.InspectCodeExePath,
                    });
                }

                var ruleRunner = new RuleRunner();

                project.RuleSet = ruleSet;
                ruleRunner.RunProject(project, (report,log) =>
                {
                    if (report != null)
                    {
                        Console.WriteLine(report.GetText());
                    }

                    if (!string.IsNullOrWhiteSpace(options.LogFile))
                    {
                        var logFile = System.IO.Path.GetFullPath(options.LogFile);

                        using (var sw = System.IO.File.AppendText(logFile))
                        {
                            sw.Write(report.GetText());
                        }
                    }
                });
            }

            Console.WriteLine("Checking is done. ");

            Console.ReadKey();
        }

        /// <summary>
        /// To check solution with StyleCop.
        /// </summary>
        /// <param name="options"></param>
        private static void CheckWithStyleCop(Options options)
        {
            Debug.WriteLine("To check with StyleCop");

            //var exePath = @"f:\_E_\jb-cl\StyleCop.exe";
            //var output = @"..\Reports\InspectCode_Report.xml";
            //var cachesHome = @"..\Reports\InspectCode_Cache\";
            //var slns = Directory.GetFiles(options.RepositoryPath, "*.sln", SearchOption.AllDirectories);
            //var uri = new Uri(options.RepositoryPath);

            //Debug.Indent();

            //foreach (var sln in slns)
            //{
            //    var uri1 = new Uri(sln);
            //    var uri2 = uri.MakeRelativeUri(uri1);

            //    var cmdLine = string.Format(@"""{0}""",
            //        uri2.ToString());

            //    Debug.WriteLine(exePath + " " + cmdLine);
            //}

            //Debug.Unindent();
        }
    }
}

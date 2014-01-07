﻿using System;
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
        /// -r "f:\_E_\SolutionTool" -d ./src ./output ./doc ./deployment -f .gitignore *.md
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

            if (!string.IsNullOrWhiteSpace(options.InspectCodePath))
            {
                if (!System.IO.File.Exists(options.InspectCodePath))
                {
                    Debug.WriteLine("Cannot find InspectCode.exe in path: " + options.InspectCodePath);

                    return;
                }
            }

            Debug.WriteLine("Begin checking " + options.RepositoryPath);

            var results = new List<Result>();

            foreach (var i in results)
            {
                Debug.WriteLine(i);
            }

            //CheckWithInspectCode(options);

            //CheckWithStyleCop(options);

            Console.WriteLine("Checking is done. ");

            Console.ReadKey();
        }

        /// <summary>
        /// To check solution with InspectCode.
        /// </summary>
        /// <example>
        /// InspectCode.exe /caches-home="C:\Temp\DFCache" /o="report.xml" "C:\src\MySolution.sln"
        /// </example>
        /// <param name="options"></param>
        private static void CheckWithInspectCode(Options options)
        {
            Debug.WriteLine("To check with InspectCode");

            var exePath = "InspectCode.exe";

            if (!System.IO.File.Exists(exePath))
            {
                Debug.WriteLine("Need to specify path of InspectCode.exe");

                return;
            }

            var cachesHome = @"..\Reports\InspectCode_Cache";
            var output = @"..\Reports\InspectCode_Report.xml";
            var slns = System.IO.Directory.GetFiles(options.RepositoryPath, "*.sln", SearchOption.AllDirectories);
            var uri = new Uri(options.RepositoryPath);

            Debug.Indent();

            foreach (var sln in slns)
            {
                var uri1 = new Uri(sln);
                var uri2 = uri.MakeRelativeUri(uri1);

                var cmdLine = string.Format(@"/caches-home=""{0}"" /o=""{1}"" ""{2}""",
                    cachesHome, output, sln);

                Debug.WriteLine(exePath + " " + cmdLine);

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = cmdLine,
                        WorkingDirectory = options.RepositoryPath,
                        ////CreateNoWindow = true,
                        //WindowStyle = ProcessWindowStyle.Hidden,
                        //RedirectStandardInput = true,
                        ////RedirectStandardError = true,
                        //RedirectStandardOutput = true,
                        //UseShellExecute = false,
                    },
                };

                proc.ErrorDataReceived += OnErrorDataReceived;
                proc.OutputDataReceived += OnOutputDataReceived;

                proc.Start();
                proc.WaitForExit();
            }

            Debug.Unindent();
        }

        static void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("Error Received: " + e.Data);
        }

        static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("Data Received: " + e.Data);
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

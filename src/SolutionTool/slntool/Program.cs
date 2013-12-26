using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Orc.SolutionTool.Properties;

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

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            CheckDirectories(options);

            CheckFiles(options);

            CheckBuildOutputPath(options);

            CheckWithInspectCode(options);

            CheckWithStyleCop(options);
        }

        /// <summary>
        /// Check if specified directoies exist. Directories are relative paths.
        /// </summary>
        /// <param name="options"></param>
        private static void CheckDirectories(Options options)
        {
            if (options.Directories != null)
            {
                Debug.WriteLine("To check directories: ");
                Debug.Indent();

                foreach (var i in options.Directories)
                {
                    var dir = Path.Combine(options.RepositoryPath, i);

                    if (!Directory.Exists(dir))
                    {
                        Debug.WriteLine(string.Format("Directory not exists: {0}", i));
                    }
                }

                Debug.Unindent();
            }
        }

        /// <summary>
        /// Check if specified files exist. Files are relative paths.
        /// </summary>
        /// <param name="options"></param>
        private static void CheckFiles(Options options)
        {
            if (options.Files != null)
            {
                Debug.WriteLine("To check files: ");
                Debug.Indent();

                foreach (var i in options.Files)
                {
                    var dir = Path.Combine(options.RepositoryPath, i);

                    if (!File.Exists(dir))
                    {
                        Debug.WriteLine(string.Format("File not exists: {0}", i));
                    }
                }

                Debug.Unindent();
            }
        }

        /// <summary>
        /// Check OutputPath of all *.csproj files in the specified reposotry.
        /// </summary>
        /// <param name="options"></param>
        private static void CheckBuildOutputPath(Options options)
        {
            if (options.CheckOutputBuildPath)
            {
                Debug.WriteLine("To check OutputPath: ");
                Debug.Indent();

                var csprojs = Directory.GetFiles(options.RepositoryPath, "*.csproj", SearchOption.AllDirectories);
                XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

                foreach (var i in csprojs)
                {
                    Debug.WriteLine(i);

                    var doc = XDocument.Load(i);
                    var eleAssemblyName = doc.Descendants(ns + "AssemblyName").First().Value;
                    var eleTargetFrameworkVersion = doc.Descendants(ns + "TargetFrameworkVersion").First().Value;
                    var eleOutputPaths = doc.Descendants(ns + "OutputPath").ToList();

                    foreach (var j in eleOutputPaths)
                    {
                        Debug.Indent();

                        Debug.WriteLine(string.Format("{0} --> {1}/{2}/.NET{3}",
                            j.Value, "./output", "TBD", eleTargetFrameworkVersion));

                        Debug.Unindent();
                    }
                }

                Debug.Unindent();
            }
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

            var exePath = Settings.Default.InspectCodeExe;

            if (!File.Exists(exePath))
            {
                Debug.WriteLine("Need to specify path of InspectCode.exe");

                return;
            }

            var cachesHome = @"..\Reports\InspectCode_Cache";
            var output = @"..\Reports\InspectCode_Report.xml";
            var slns = Directory.GetFiles(options.RepositoryPath, "*.sln", SearchOption.AllDirectories);
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

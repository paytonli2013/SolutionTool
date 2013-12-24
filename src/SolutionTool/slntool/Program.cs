using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
            Trace.Listeners.Add(new ConsoleTraceListener());

            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            CheckDirectories(options);

            CheckFiles(options);

            CheckBuildOutputPath(options);
        }

        /// <summary>
        /// Check if specified directoies exist. Directories are relative paths.
        /// </summary>
        /// <param name="options"></param>
        private static void CheckDirectories(Options options)
        {
            if (options.Directories != null)
            {
                Trace.WriteLine("To check directories: ");
                Trace.Indent();

                foreach (var i in options.Directories)
                {
                    var dir = Path.Combine(options.RepositoryPath, i);

                    if (!Directory.Exists(dir))
                    {
                        Trace.WriteLine(string.Format("Directory not exists: {0}", i));
                    }
                }

                Trace.Unindent();
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
                Trace.WriteLine("To check files: ");
                Trace.Indent();

                foreach (var i in options.Files)
                {
                    var dir = Path.Combine(options.RepositoryPath, i);

                    if (!File.Exists(dir))
                    {
                        Trace.WriteLine(string.Format("File not exists: {0}", i));
                    }
                }

                Trace.Unindent();
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
                Trace.WriteLine("To check OutputPath: ");
                Trace.Indent();

                var csprojs = Directory.GetFiles(options.RepositoryPath, "*.csproj", SearchOption.AllDirectories);
                XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

                foreach (var i in csprojs)
                {
                    Trace.WriteLine(i);

                    var doc = XDocument.Load(i);
                    var eleAssemblyName = doc.Descendants(ns + "AssemblyName").First().Value;
                    var eleTargetFrameworkVersion = doc.Descendants(ns + "TargetFrameworkVersion").First().Value;
                    var eleOutputPaths = doc.Descendants(ns + "OutputPath").ToList();

                    foreach (var j in eleOutputPaths)
                    {
                        Trace.Indent();

                        Trace.WriteLine(string.Format("{0} --> {1}/{2}/.NET{3}",
                            j.Value, "./output", "TBD", eleTargetFrameworkVersion));

                        Trace.Unindent();
                    }
                }

                Trace.Unindent();
            }
        }
    }
}

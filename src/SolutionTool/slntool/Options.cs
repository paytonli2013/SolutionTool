using CommandLine;
using CommandLine.Text;

namespace Orc.SolutionTool
{
    class Options
    {
        [Option('r', "repository", Required = true, HelpText = "The path of the repository. ")]
        public string RepositoryPath { get; set; }

        [OptionArray('d', "directories", Required = false,
            //DefaultValue = new List<string> { "./src", "./output", "./doc", "./deployment", },
            HelpText = "Directories to be checked. ")]
        public string[] Directories { get; set; }

        [OptionArray('f', "files", Required = false,
            //DefaultValue = new List<string> { "Settings.StyleCop", "Readme.md", },
            HelpText = "Files to be checked. ")]
        public string[] Files { get; set; }

        [Option('b', "builds", Required = false, DefaultValue = true, 
            HelpText = "Check the output build path. ")]
        public bool CheckOutputBuildPath { get; set; }

        [Option('i', "inspectcode", Required = false, 
            HelpText = "To check with InspectCode, specify the path to InspectCode.exe. ")]
        public string InspectCodePath { get; set; }

        //[Option('c', "stylecop", Required = false,
        //    HelpText = "To check with StyleCop, specify the path to StyleCop.exe. ")]
        //public string StyleCopPath { get; set; }

        [Option('l', "log", Required = false, DefaultValue = "report.log",
            HelpText = "The path of log file. ")]
        public string LogFile { get; set; }

        [Option('v', "verbose", DefaultValue = true, 
            HelpText = "Prints all messages to standard output. ")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}

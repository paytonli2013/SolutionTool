using CommandLine;
using CommandLine.Text;

namespace Orc.SolutionTool
{
    class Options
    {
        [Option('r', "repository", Required = true, HelpText = "The path of the repository. ")]
        public string Repository { get; set; }

        [Option('t', "template", Required = false, DefaultValue = "default.xml",
            HelpText = "The xml template file name for checking solution structure. ")]
        public string Template { get; set; }

        //[Option('b', "builds", Required = false, DefaultValue = true, 
        //    HelpText = "Check the output build path. ")]
        //public bool CheckOutputBuildPath { get; set; }

        //[Option('i', "inspectcode", Required = false, 
        //    HelpText = "To check with InspectCode, specify the path to InspectCode.exe. ")]
        //public string InspectCodePath { get; set; }

        //[Option('c', "stylecop", Required = false,
        //    HelpText = "To check with StyleCop, specify the path to StyleCop.exe. ")]
        //public string StyleCopPath { get; set; }

        //[Option('l', "log", Required = false, DefaultValue = "report.log",
        //    HelpText = "The path of log file. ")]
        //public string LogFile { get; set; }

        //[Option('v', "verbose", DefaultValue = true, 
        //    HelpText = "Prints all messages to standard output. ")]
        //public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}

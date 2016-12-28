using CommandLine;

namespace NugetXray
{
    public class Command
    {
        [Option('o', "outputFile", Required = false, HelpText = "The path to write the report to.")]
        public string OutputFile { get; set; }

        [Option('f', "OutputFormat", Required = false, HelpText = "The format of the report to write: Text or Json.", Default = ReportFormat.Text)]
        public ReportFormat OutputFormat { get; set; }
    }

    public enum ReportFormat
    {
        Text,
        Json,
        Html
    }
}
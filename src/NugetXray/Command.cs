using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
#pragma warning disable 168

namespace NugetXray
{
    public abstract class Command : ICommandValidator
    {
        [Option('o', "outputFile", Required = false, HelpText = "The path to write the report to.")]
        public string OutputFile { get; set; }

        [Option('f', "OutputFormat", Required = false, HelpText = "The format of the report to write: Text or Json.", Default = "Text")]
        public string OutputFormat { get; set; }

        public virtual IEnumerable<string> GetErrors()
        {
            if (!Enum.TryParse(OutputFormat, true, out ReportFormat val))
            {
                yield return "Invalid outputFormat.";
            }

            if (!string.IsNullOrEmpty(OutputFile) && !new FileInfo(OutputFile).Directory.Exists)
            {
                yield return $"Invalid output file: {OutputFile}. Directory does not exist.";
            }
        }
    }

    public enum ReportFormat
    {
        Text,
        Json,
        Html
    }
}
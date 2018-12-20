using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace NugetXray.Diff
{
    [Verb("diff", HelpText = "")]
    class PackageDiffCommand : Command
    {
        [Option('d', "directory", Required = true, HelpText = "The directory to be recursively scanned for packages.config.")]
        public string Directory { get; set; }

        [Option('s', "source", Required = false, HelpText = "The package source to compare against.", Default = "https://www.nuget.org/api/v2")]
        public string Source { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
        
        public override string ToString()
        {
            return $"Diff {Directory}";
        }

        public override IEnumerable<string> GetErrors()
        {
            foreach (var error in base.GetErrors())
            {
                yield return error;
            }

            if (!new DirectoryInfo(Directory).Exists)
                yield return $"Directory {Directory} does not exist.";

            if(!Uri.IsWellFormedUriString(Source, UriKind.RelativeOrAbsolute))
                yield return $"Uri {Source} is not a valid location.";
        }
    }
}
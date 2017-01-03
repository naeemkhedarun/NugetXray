using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace NugetXray.Duplicate
{
    [Verb("duplicate", HelpText = "Find references to multiple versions of the same package.")]
    class PackageDuplicateCommand : Command
    {
        [Option('d', "directory", Required = true, HelpText = "The directory to be recursively scanned for packages.config.")]
        public string Directory { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        public override string ToString()
        {
            return $"Duplicate {Directory}";
        }

        public override IEnumerable<string> GetErrors()
        {
            foreach (var error in base.GetErrors())
            {
                yield return error;
            }

            if(!new DirectoryInfo(Directory).Exists)
                yield return $"Directory {Directory} does not exist.";
        }
    }
}
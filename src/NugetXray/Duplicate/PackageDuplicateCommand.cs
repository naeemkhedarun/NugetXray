using CommandLine;

namespace NugetXray.Duplicate
{
    [Verb("duplicate", HelpText = "Find references to multiple versions of the same package.")]
    class PackageDuplicateCommand
    {
        [Option('d', "directory", Required = true, HelpText = "The directory to be recursively scanned for packages.config.")]
        public string Directory { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        public override string ToString()
        {
            return $"Duplicate {Directory}";
        }
    }
}
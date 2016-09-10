using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<int> RunAsync()
        {
            Console.WriteLine($"Scanning {Directory} for packages.configs.");

            try
            {
                var scanner = new PackageConfigurationScanner();
                var configs = scanner.Find(Directory);
                var reader = new BulkPackageConfigurationReader();
                var packages = await reader.GetPackagesAsync(configs.ToArray());
                var duplicates = new PackageDuplicateDetector().FindDuplicates(packages)
                                                               .OrderByDescending(x => x.Versions.Length);

                new ConsolePackageDuplicateReport(duplicates, Verbose).Write();
            }
            catch (Exception e)
            {
                Console.WriteLine(Verbose ? e.ToString() : e.Message);

                return 1;
            }

            return 0;
        }
    }
}
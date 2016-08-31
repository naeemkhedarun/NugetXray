using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace NugetXray.Diff
{
    [Verb("diff", HelpText = "")]
    class PackageDiffCommand
    {
        [Option('d', "directory", Required = true, HelpText = "The directory to be recursively scanned for packages.config.")]
        public string Directory { get; set; }

        [Option('s', "source", Required = false, HelpText = "The package source to compare against.", Default = "https://www.nuget.org/api/v2")]
        public string Source { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        public async Task<int> RunAsync()
        {
            Console.WriteLine($"Scanning {Source} for packages.configs.");

            var scanner = new PackageConfigurationScanner();
            var configs = scanner.Find(Directory);
            var reader = new BulkPackageConfigurationReader();
            var packages = await reader.GetPackagesAsync(configs.ToArray());
            var consolidatedPackages = PackageReferenceConsolidator.Consolidate(packages);
            var differ = new PackageConfigurationVersionDiffer(Source);
            var results = consolidatedPackages.Zip(
                    await differ.GetPackageDiffsAsync(consolidatedPackages.Select(x => x.Key).ToArray()),
                    (pair, diff) => new PackageDiffReportItem(pair.Key, pair.Value, diff))
                .OrderByDescending(x => x.Diff.Diff);

            new ConsolePackageDiffReport(results, Verbose).Write();

            return 0;
        }
    }
}
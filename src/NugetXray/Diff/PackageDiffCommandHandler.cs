using System;
using System.Linq;
using System.Threading.Tasks;

namespace NugetXray.Diff
{
    class PackageDiffCommandHandler : ICommand
    {
        private readonly Type _type = typeof(PackageDiffCommand);

        public bool CanHandle(Type command)
        {
            return _type == command;
        }

        public async Task<CommandResult> Execute(object command)
        {
            var packageDiffCommand = (PackageDiffCommand)command;
            Console.WriteLine($"Scanning {packageDiffCommand.Source} for packages.configs.");

            try
            {
                var scanner = new PackageConfigurationScanner();
                var configs = scanner.Find(packageDiffCommand.Directory);
                var reader = new BulkPackageConfigurationReader();
                var packages = await reader.GetPackagesAsync(configs.ToArray());
                var consolidatedPackages = PackageReferenceConsolidator.Consolidate(packages);
                var differ = new PackageConfigurationVersionDiffer(packageDiffCommand.Source);
                var results = consolidatedPackages.Zip(
                        await differ.GetPackageDiffsAsync(consolidatedPackages.Select(x => x.Key).ToArray()),
                        (pair, diff) => new PackageDiffReportItem(pair.Key, pair.Value, diff))
                    .OrderByDescending(x => x.Diff.Diff);

                var consolePackageDiffReport = new ConsolePackageDiffReport(results, packageDiffCommand.Verbose);
                return new CommandResult(consolePackageDiffReport, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(packageDiffCommand.Verbose ? e.ToString() : e.Message);

                return new CommandResult(null, 1);
            }
        }

        public Type CommandType => _type;
    }
}
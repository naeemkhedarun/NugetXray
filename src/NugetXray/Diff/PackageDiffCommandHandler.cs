using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;

namespace NugetXray.Diff
{
    class PackageDiffCommandHandler : ICommand
    {
        private readonly CachedPackageReader _cachedPackageReader;
        private readonly Type _type = typeof(PackageDiffCommand);

        public PackageDiffCommandHandler(CachedPackageReader cachedPackageReader)
        {
            _cachedPackageReader = cachedPackageReader;
        }

        public bool CanHandle(Type command)
        {
            return _type == command;
        }

        public async Task<CommandResult> Execute(object command)
        {
            var packageDiffCommand = (PackageDiffCommand) command;

            Console.WriteLine($"Scanning {packageDiffCommand.Directory} against {packageDiffCommand.Source}.");

            try
            {
                var packages = await _cachedPackageReader.GetPackagesAsync(packageDiffCommand.Directory);

                var consolidatedPackages = PackageReferenceConsolidator.Consolidate(packages);
                var differ = new PackageConfigurationVersionDiffer(packageDiffCommand.Source);
                var results = consolidatedPackages.Zip(
                        await differ.GetPackageDiffsAsync(consolidatedPackages.Select(x => x.Key).ToArray()),
                        (pair, diff) => new PackageDiffReportItem(pair.Key, pair.Value, diff))
                    .OrderByDescending(x => x.Diff.Diff);

                var consolePackageDiffReport = new ConsolePackageDiffReport(results, packageDiffCommand.Verbose);
                return new CommandResult(consolePackageDiffReport, 0, packageDiffCommand.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(packageDiffCommand.Verbose ? e.ToString() : e.Message);

                return new CommandResult(null, 1, packageDiffCommand.ToString());
            }
        }

        public Type CommandType => _type;
    }
}
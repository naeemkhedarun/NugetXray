using System;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Versioning;

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

        public async Task<int> Execute(object command)
        {
            var packageDiffCommand = (PackageDiffCommand) command;

            Console.WriteLine($"Scanning {packageDiffCommand.Directory} against {packageDiffCommand.Source}.{Environment.NewLine}");
            
            try
            {
                var packages = await _cachedPackageReader.GetPackagesAsync(packageDiffCommand.Directory);

                var consolidatedPackages = PackageReferenceConsolidator.Consolidate(packages);
                var differ = new PackageConfigurationVersionDiffer(packageDiffCommand.Source);
                var results = consolidatedPackages.Zip(
                        await differ.GetPackageDiffsAsync(consolidatedPackages.Select(x => x.Key).ToArray()),
                        (pair, diff) => new PackageDiffReportItem(pair.Key, pair.Value, diff))
                    .OrderByDescending(x => x.Diff.Diff);

                var textReport = new TextPackageDiffReport().CreateReport(results, packageDiffCommand.Verbose);
                Console.WriteLine(string.Join(Environment.NewLine, textReport));

                new ReportWriter().WriteReport(packageDiffCommand, () =>
                {
                    return string.Join(Environment.NewLine, textReport);
                }, () =>
                {
                    return results.Where(x => x.Diff.Diff > new SemanticVersion(0, 0, 0));
                }, () =>
                {
                    return results.Where(x => x.Diff.Diff > new SemanticVersion(0, 0, 0));
                });

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(packageDiffCommand.Verbose ? e.ToString() : e.Message);

                return -1;
            }
        }

        public Type CommandType => _type;
    }
}
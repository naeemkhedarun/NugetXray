using System;
using System.Linq;
using System.Threading.Tasks;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    class PackageDuplicateCommandHandler : ICommand
    {
        private readonly CachedPackageReader _cachedPackageReader;
        private readonly Type _type = typeof(PackageDuplicateCommand);

        public PackageDuplicateCommandHandler(CachedPackageReader cachedPackageReader)
        {
            _cachedPackageReader = cachedPackageReader;
        }

        public bool CanHandle(Type command)
        {
            return _type == command;
        }

        public async Task<int> Execute(object command)
        {
            var packageDuplicateCommand = (PackageDuplicateCommand) command;
            Console.WriteLine($"Scanning {packageDuplicateCommand.Directory} for packages.configs.{Environment.NewLine}");

            try
            {
                var packages = await _cachedPackageReader.GetPackagesAsync(packageDuplicateCommand.Directory);

                var duplicates = new PackageDuplicateDetector().FindDuplicates(packages)
                    .OrderByDescending(x => x.Versions.Length);

                var textReport = new TextPackageDuplicateReport().CreateReport(duplicates, packageDuplicateCommand.Verbose);
                Console.WriteLine(string.Join(Environment.NewLine, textReport));

                new ReportWriter().WriteReport(packageDuplicateCommand, () =>
                {
                    return string.Join(Environment.NewLine, textReport);
                }, () =>
                {
                    return duplicates;
                }, () =>
                {
                    return duplicates;
                });

                return duplicates.Count() * -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(packageDuplicateCommand.Verbose ? e.ToString() : e.Message);
                return -1;
            }
        }

        public Type CommandType => _type;
    }
}
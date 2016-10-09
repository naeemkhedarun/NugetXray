using System;
using System.Linq;
using System.Threading.Tasks;

namespace NugetXray.Duplicate
{
    class PackageDuplicateCommandHandler : ICommand
    {
        private readonly Type _type = typeof(PackageDuplicateCommand);

        public bool CanHandle(Type command)
        {
            return _type == command;
        }

        public async Task<CommandResult> Execute(object command)
        {
            var packageDuplicateCommand = (PackageDuplicateCommand) command;
            Console.WriteLine($"Scanning {packageDuplicateCommand.Directory} for packages.configs.");
            
            try
            {
                var scanner = new PackageConfigurationScanner();
                var configs = scanner.Find(packageDuplicateCommand.Directory);
                var reader = new BulkPackageConfigurationReader();
                var packages = await reader.GetPackagesAsync(configs.ToArray());
                var duplicates = new PackageDuplicateDetector().FindDuplicates(packages)
                    .OrderByDescending(x => x.Versions.Length);

                var report = new TextPackageDuplicateReport(duplicates, packageDuplicateCommand.Verbose);
                return new CommandResult(report, duplicates.Any() ? 1 : 0, packageDuplicateCommand.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(packageDuplicateCommand.Verbose ? e.ToString() : e.Message);
                return new CommandResult(null, 1, packageDuplicateCommand.ToString());
            }
        }

        public Type CommandType => _type;
    }
}
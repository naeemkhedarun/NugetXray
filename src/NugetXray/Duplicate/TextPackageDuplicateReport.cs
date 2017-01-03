using System.Linq;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class TextPackageDuplicateReport : TextReport
    {
        private readonly IOrderedEnumerable<PackageDuplicate> _results;
        private readonly bool _verbose;

        public TextPackageDuplicateReport(IOrderedEnumerable<PackageDuplicate> results, bool verbose)
        {
            _results = results;
            _verbose = verbose;
        }

        protected override void CreateReport()
        {
            var errors = 0;

            foreach (var packageDuplicate in _results)
            {
                errors++;

                var diffMessage = $"{packageDuplicate.Versions.Select(x => x.SemanticVersion).Distinct().Count()} versions";
                
                if (_verbose)
                {
                    WriteError($"{packageDuplicate.PackageReference.PackageIdentity.Id} | {diffMessage}");
                    
                    var groupedByVersion = packageDuplicate.Versions.GroupBy(x => x.SemanticVersion).OrderBy(x => x.Key);
                    foreach (var version in groupedByVersion)
                    {
                        WriteError($"  {version.Key}");
                        foreach (var duplicateVersion in version)
                        {
                            WriteError($"    {duplicateVersion.PackageConfig}");
                        }
                    }
                }
                else
                {
                    WriteError($"{packageDuplicate.PackageReference.PackageIdentity.Id.PadRight(70)} | {diffMessage.PadRight(10)}");
                }
            }

            Write(string.Empty);
            WriteError($"Errors: {errors}");
            Write(string.Empty);
        }
    }
}
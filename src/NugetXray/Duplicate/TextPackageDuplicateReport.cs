using System.Collections.Generic;
using System.Linq;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class TextPackageDuplicateReport
    {
        public List<string> CreateReport(IOrderedEnumerable<PackageDuplicate> results, bool verbose)
        {
            var messages = new List<string>();

            var errors = 0;

            foreach (var packageDuplicate in results)
            {
                errors++;

                var diffMessage = $"{packageDuplicate.Versions.Select(x => x.SemanticVersion).Distinct().Count()} versions";
                
                if (verbose)
                {
                    messages.Add($"{packageDuplicate.PackageReference.PackageIdentity.Id} | {diffMessage}");
                    
                    var groupedByVersion = packageDuplicate.Versions.GroupBy(x => x.SemanticVersion).OrderBy(x => x.Key);
                    foreach (var version in groupedByVersion)
                    {
                        messages.Add($"  {version.Key}");
                        foreach (var duplicateVersion in version)
                        {
                            messages.Add($"    {duplicateVersion.PackageConfig}");
                        }
                    }
                }
                else
                {
                    messages.Add($"{packageDuplicate.PackageReference.PackageIdentity.Id.PadRight(70)} | {diffMessage.PadRight(10)}");
                }
            }

            messages.Add(string.Empty);
            messages.Add($"Errors: {errors}");
            messages.Add(string.Empty);

            return messages;
        }
    }
}
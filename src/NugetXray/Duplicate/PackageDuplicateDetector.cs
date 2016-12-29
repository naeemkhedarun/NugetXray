using System.Collections.Generic;
using System.Linq;
using NugetXray.Diff;
using NuGet.Packaging;

namespace NugetXray.Duplicate
{
    public class PackageDuplicateDetector
    {
        public PackageDuplicate[] FindDuplicates(Dictionary<string, PackageReference[]> packages)
        {
            var groupedById = packages.SelectMany(x => x.Value.Select(y => new {PackageReference = y, Config = x.Key}))
                                      .GroupBy(x => x.PackageReference.PackageIdentity.Id);

            var duplicates = groupedById.Where(x => x.Select(y => y.PackageReference.PackageIdentity.Version).Distinct().Count() > 1);

            return duplicates.Select(x => new PackageDuplicate(
                x.First().PackageReference, 
                x.Select(y => new DuplicateVersion(y.Config, y.PackageReference.PackageIdentity.Version)).ToArray())).ToArray();
        }
    }
}
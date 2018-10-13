using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;

namespace NugetXray
{
    public class PackageReferenceConsolidator
    {
        public static Dictionary<PackageReference, string[]> Consolidate(Dictionary<string, PackageReference[]> packages)
        {
            var invertedPackages = packages.SelectMany(x => x.Value.Select(y => new { Config = x.Key, Package = y }))
                .GroupBy(x => x.Package.PackageIdentity.ToString())
                .ToDictionary(x => x.First().Package,  y => y.Select(z => z.Config).ToArray());
            return invertedPackages;
        }
    }
}
using System.Linq;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    public static class PackageVersionDiffer
    {
        public static PackageDiff GetVersionDiff(PackageReference packageReference, NuGetVersion[] versions)
        {
            var current = packageReference.PackageIdentity.Version;
            if(!versions.Any())
                return new PackageDiff(packageReference);

            var filteredWhenStable = current.IsPrerelease ? versions.Where(x => x.Release == current.Release || !x.IsPrerelease) : versions.Where(x => !x.IsPrerelease);
            var latest = filteredWhenStable.OrderByDescending(x => x).First();
            
            if (latest.Major != current.Major)
                return new PackageDiff(packageReference, latest, new SemanticVersion(latest.Major - current.Major, 0, 0));
            if (latest.Minor != current.Minor)
                return new PackageDiff(packageReference, latest, new SemanticVersion(0, latest.Minor - current.Minor, 0));
            return new PackageDiff(packageReference, latest, new SemanticVersion(0, 0, latest.Patch - current.Patch));
        }
    }
}
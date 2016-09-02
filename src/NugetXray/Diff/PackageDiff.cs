using NuGet.Packaging;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    public class PackageDiff
    {
        public PackageReference PackageReference { get; set; }
        public SemanticVersion Current { get; set; }
        public SemanticVersion Latest { get; set; }
        public SemanticVersion Diff { get; set; }

        public PackageDiff(PackageReference packageReference, SemanticVersion latest, SemanticVersion diff)
        {
            PackageReference = packageReference;
            Current = packageReference.PackageIdentity.Version;
            Latest = latest;
            Diff = diff;
        }

        public PackageDiff(PackageReference packageReference) : this(
            packageReference, 
            new SemanticVersion(0,0,0), 
            new SemanticVersion(0,0,0))
        {
            PackageReference = packageReference;
        }

        public bool WasFoundInFeed => Latest > new SemanticVersion(0, 0, 0);
    }
}
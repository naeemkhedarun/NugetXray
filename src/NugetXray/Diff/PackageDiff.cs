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

        public PackageDiff(PackageReference packageReference, SemanticVersion current, SemanticVersion latest, SemanticVersion diff)
        {
            PackageReference = packageReference;
            Current = current;
            Latest = latest;
            Diff = diff;
        }
    }
}
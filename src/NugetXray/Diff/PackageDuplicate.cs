using NuGet.Packaging;

namespace NugetXray.Diff
{
    public class PackageDuplicate
    {
        public PackageReference PackageReference { get; set; }
        public DuplicateVersion[] Versions { get; set; }

        public PackageDuplicate(PackageReference packageReference, DuplicateVersion[] versions)
        {
            PackageReference = packageReference;
            Versions = versions;
        }
    }
}
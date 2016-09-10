using System;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    public class PackageDuplicate
    {
        public PackageReference PackageReference { get; set; }
        public Tuple<SemanticVersion, string>[] Versions { get; set; }

        public PackageDuplicate(PackageReference packageReference, Tuple<SemanticVersion, string>[] versions)
        {
            PackageReference = packageReference;
            Versions = versions;
        }
    }
}
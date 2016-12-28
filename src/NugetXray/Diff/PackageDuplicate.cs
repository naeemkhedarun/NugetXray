using System;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    public class DuplicateVersion
    {
        public DuplicateVersion(string packageConfig, SemanticVersion semanticVersion)
        {
            PackageConfig = packageConfig;
            SemanticVersion = semanticVersion;
        }

        public string PackageConfig { get; private set; }
        public SemanticVersion SemanticVersion { get; private set; }
    }

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
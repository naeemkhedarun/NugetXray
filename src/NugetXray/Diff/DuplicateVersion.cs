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
}
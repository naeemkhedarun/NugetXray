using NugetXray.Diff;
using NuGet.Packaging;

namespace NugetXray.Diff
{
    public class PackageDiffReportItem
    {
        public PackageReference Package { get; }
        public string[] Configs { get; }
        public PackageDiff Diff { get; }

        public PackageDiffReportItem(PackageReference package, string[] configs, PackageDiff diff)
        {
            Package = package;
            Configs = configs;
            Diff = diff;
        }
    }
}
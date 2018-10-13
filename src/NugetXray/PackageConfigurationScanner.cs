using System.IO;
using System.Linq;

namespace NugetXray
{
    public class PackageConfigurationScanner
    {
        public string[] Find(string rootPath, PackageLocation packageLocation)
        {
            var filesToScan = packageLocation == PackageLocation.Csproj ? "*.csproj" : "packages.config";
            var rootDirectory = new DirectoryInfo(rootPath);
            var files = rootDirectory.GetFiles(filesToScan, SearchOption.AllDirectories);
            return files.Select(x => x.FullName).ToArray();
        }
    }
}
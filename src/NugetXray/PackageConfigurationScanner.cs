using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NugetXray
{
    public class PackageConfigurationScanner
    {
        public string[] Find(string rootPath)
        {
            var rootDirectory = new DirectoryInfo(rootPath);
            var files = rootDirectory.GetFiles("packages.config", SearchOption.AllDirectories);
            return files.Select(x => x.FullName).ToArray();
        }
    }
}
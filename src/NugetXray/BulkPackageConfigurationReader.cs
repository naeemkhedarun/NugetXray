using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NugetXray
{
    public class BulkPackageConfigurationReader
    {
        public async Task<Dictionary<string, PackageReference[]>> GetPackagesAsync(
            string[] packageConfigurationPaths, 
            PackageLocation packageLocation)
        {
            if (packageLocation == PackageLocation.Csproj)
                return await PackageReferencesesFromCsproj(packageConfigurationPaths);

            return await PackageReferencesesFromPackagesConfig(packageConfigurationPaths);
        }

        private static async Task<Dictionary<string, PackageReference[]>> PackageReferencesesFromCsproj(string[] packageConfigurationPaths)
        {
            var tasks = packageConfigurationPaths.ToDictionary(path => path, path => Task.Run(() =>
            {
                try
                {
                    var csprojStream = new StreamReader(path);

                    var doc = XDocument.Parse(csprojStream.ReadToEnd());
                    var targetFrameWork = doc.XPathSelectElement("//TargetFramework");
                    IEnumerable<PackageReference> packageReferences = doc.XPathSelectElements("//PackageReference")
                        .Select(pr => new PackageReference(
                            GetPackageIdentity(pr), 
                            new NuGetFramework(targetFrameWork.Value)));

                    return packageReferences;
                }
                catch (Exception e)
                {
                    throw new PackagesConfigReaderException($"Failed on {path}", e);
                }
            }));

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(x => x.Key, x => x.Value.Result.ToArray());
        }

        private static async Task<Dictionary<string, PackageReference[]>> PackageReferencesesFromPackagesConfig(string[] packageConfigurationPaths)
        {
            var tasks = packageConfigurationPaths.ToDictionary(path => path, path => Task.Run(() =>
            {
                try
                {
                    var packageReferences = new PackagesConfigReader(new FileStream(path, FileMode.Open, FileAccess.Read)).GetPackages();
                    return packageReferences;
                }
                catch (Exception e)
                {
                    throw new PackagesConfigReaderException($"Failed on {path}", e);
                }
            }));

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(x => x.Key, x => x.Value.Result.ToArray());
        }

        private static PackageIdentity GetPackageIdentity(XElement pr)
        {
            return new PackageIdentity(pr.Attribute("Include").Value, new NuGetVersion(pr.Attribute("Version").Value));
        }
    }
}
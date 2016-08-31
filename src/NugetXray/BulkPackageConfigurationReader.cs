using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;

namespace NugetXray
{
    public class BulkPackageConfigurationReader
    {
        public async Task<Dictionary<string, PackageReference[]>> GetPackagesAsync(string[] packageConfigurationPaths)
        {
            var tasks = packageConfigurationPaths.ToDictionary(path => path, path => 
                Task.Run(() => new PackagesConfigReader(new FileStream(path, FileMode.Open, FileAccess.Read)).GetPackages())
            );

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(x => x.Key, x => x.Value.Result.ToArray());
        }
    }
}
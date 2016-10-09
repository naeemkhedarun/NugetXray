using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;

namespace NugetXray.Diff
{
    public class CachedPackageReader
    {
        private readonly object _objectLock = new object();

        private readonly Dictionary<string, Task<Dictionary<string, PackageReference[]>>> _cache 
            = new Dictionary<string, Task<Dictionary<string, PackageReference[]>>>();

        public async Task<Dictionary<string, PackageReference[]>> GetPackagesAsync(string directory)
        {
            lock (_objectLock)
            {
                if (!_cache.ContainsKey(directory))
                {
                    _cache[directory] = Task.Run(async () =>
                    {
                        var scanner = new PackageConfigurationScanner();
                        var configs = scanner.Find(directory);
                        var reader = new BulkPackageConfigurationReader();
                        var packages = await reader.GetPackagesAsync(configs.ToArray());
                        return packages;
                    });
                }
            }

            return await _cache[directory];
        }
    }
}
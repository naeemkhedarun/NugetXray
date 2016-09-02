using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NugetXray.Diff
{
    public class PackageConfigurationVersionDiffer
    {
        private readonly string _packageSource;

        public PackageConfigurationVersionDiffer(string packageSource)
        {
            _packageSource = packageSource;
        }

        public async Task<PackageDiff[]> GetPackageDiffsAsync(PackageReference[] packages)
        {
            var repo = Repository.Factory.GetCoreV2(new PackageSource(_packageSource));
            var resource = await repo.GetResourceAsync<FindPackageByIdResource>(CancellationToken.None);
            resource.CacheContext = new SourceCacheContext();

            var packageVersions = packages.Select(x =>
                    new { PackageReference = x, Task = resource.GetAllVersionsAsync(x.PackageIdentity.Id, CancellationToken.None) }).ToList();
            await Task.WhenAll(packageVersions.Select(x => x.Task));

            return packageVersions.Select(x => PackageVersionDiffer.GetVersionDiff(x.PackageReference, x.Task.Result.ToArray())).ToArray();
        }
    }
}
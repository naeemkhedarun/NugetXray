using System.Linq;
using System.Threading.Tasks;
using NugetXray.Diff;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NugetXray.Tests
{
    public class PackageConfigurationVersionDifferTests
    {
        [Fact]
        public async Task CanReturnVersionDifferences()
        {
            var differ = new PackageConfigurationVersionDiffer("https://www.nuget.org/api/v2");

            var diffs = await differ.GetPackageDiffsAsync(new[]
            {
                new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework), 
                new PackageReference(new PackageIdentity("Castle.Core", new NuGetVersion("1.0.0")), NuGetFramework.AnyFramework), 
            });

            Assert.Equal(2, diffs.Count());
        }
    }
}
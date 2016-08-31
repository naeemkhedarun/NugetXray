using System.Threading.Tasks;
using Xunit;

namespace NugetXray.Tests
{
    public class BulkPackageConfigurationReaderTests
    {
        [Fact]
        public async Task CanBulkReadPackages()
        {
            var reader = new BulkPackageConfigurationReader();
            var packages = await reader.GetPackagesAsync(new[]
            {
                @"MockRootFolder\packages.config",
                @"MockRootFolder\MockSubfolderOne\packages.config",
                @"MockRootFolder\MockSubfolderTwo\packages.config",
            });

            Assert.NotEmpty(packages);
        }
    }
}
using System.IO;
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
            }, PackageLocation.Packages);

            Assert.NotEmpty(packages);
        }

        [Fact]
        public async Task CanBulkReadCsproj()
        {
            var folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var reader = new BulkPackageConfigurationReader();
            var packages = await reader.GetPackagesAsync(new[]
            {
                folder + @"\MockRootFolder\mockproject.csproj",
                folder + @"\MockRootFolder\MockSubfolderOne\mockproject.csproj",
                folder + @"\MockRootFolder\MockSubfolderTwo\mockproject.csproj"
            }, PackageLocation.Csproj);

            Assert.NotEmpty(packages);
        }
    }
}
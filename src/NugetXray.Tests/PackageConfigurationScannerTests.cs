using System.IO;
using System.Linq;
using Xunit;

namespace NugetXray.Tests
{
    public class PackageConfigurationScannerTests
    {
        [Fact]
        public void CanFindPackageConfigurations()
        {
            var scanner = new PackageConfigurationScanner();

            var results = scanner.Find("MockRootFolder", PackageLocation.Packages)
                .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty));

            Assert.Equal(new []
            {
                @"\MockRootFolder\packages.config",
                @"\MockRootFolder\MockSubfolderOne\packages.config",
                @"\MockRootFolder\MockSubfolderTwo\packages.config",
            }, results);
        }

        [Fact]
        public void CanFindCsprojConfigurations()
        {
            var scanner = new PackageConfigurationScanner();

            var results = scanner.Find("MockRootFolder", PackageLocation.Csproj)
                .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty));

            Assert.Equal(new []
            {
                @"\MockRootFolder\mockproject.csproj",
                @"\MockRootFolder\MockSubfolderOne\mockproject.csproj",
                @"\MockRootFolder\MockSubfolderTwo\mockproject.csproj"
            }, results);
        }
    }
}


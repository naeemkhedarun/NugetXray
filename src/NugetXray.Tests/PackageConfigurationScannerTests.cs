using System.IO;
using Xunit;
using System.Linq;

namespace NugetXray.Tests
{
    public class PackageConfigurationScannerTests
    {
        [Fact]
        public void CanFindPackageConfigurations()
        {
            var scanner = new PackageConfigurationScanner();

            var results = scanner.Find("MockRootFolder")
                .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty));

            Assert.Equal(new []
            {
                @"\MockRootFolder\packages.config",
                @"\MockRootFolder\MockSubfolderOne\packages.config",
                @"\MockRootFolder\MockSubfolderTwo\packages.config",
            }, results);
        }
    }
}


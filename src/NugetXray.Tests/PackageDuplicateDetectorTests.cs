using System.Collections.Generic;
using NugetXray.Duplicate;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NugetXray.Tests
{
    public class PackageDuplicateDetectorTests
    {
        private readonly PackageDuplicateDetector _detector;

        public PackageDuplicateDetectorTests()
        {
            _detector = new PackageDuplicateDetector();
        }

        [Fact]
        public void CanFindDuplicates()
        {
            var results = _detector.FindDuplicates(new Dictionary<string, PackageReference[]>
            {
                { "ConfigOne" , new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework)}},
                { "ConfigTwo" , new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.0")), NuGetFramework.AnyFramework)}}
            });

            Assert.NotEmpty(results);
        }

        [Fact]
        public void CanDifferentiatePackageIds()
        {
            var results = _detector.FindDuplicates(new Dictionary<string, PackageReference[]>
            {
                { "ConfigOne" , new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework)}},
                { "ConfigTwo" , new [] {new PackageReference(new PackageIdentity("Castle.Core", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework)}}
            });

            Assert.Empty(results);
        }

        [Fact]
        public void CanIgnoreSameVersionInDifferentConfigsIds()
        {
            var results = _detector.FindDuplicates(new Dictionary<string, PackageReference[]>
            {
                { "ConfigOne" , new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework)}},
                { "ConfigTwo" , new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework)}}
            });

            Assert.Empty(results);
        }
    }
}
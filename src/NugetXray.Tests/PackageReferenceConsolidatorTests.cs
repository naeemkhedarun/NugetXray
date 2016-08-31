using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NugetXray.Tests
{
    public class PackageReferenceConsolidatorTests
    {
        [Fact]
        public void CanConsolidatePackages()
        {
            var packages = new Dictionary<string, PackageReference[]>
            {
                { "ProjectOne", new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework) }},
                { "ProjectTwo", new [] {new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework) }},
            };

            var result = PackageReferenceConsolidator.Consolidate(packages);
            Assert.Equal(1, result.Count);
        }
    }
}
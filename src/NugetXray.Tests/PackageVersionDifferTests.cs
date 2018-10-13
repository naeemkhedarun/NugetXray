using NugetXray.Diff;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NugetXray.Tests
{
    public class PackageVersionDifferTests
    {
        [Fact]
        public void CanReturnStableVersionDifference()
        {
            var packageReference = new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1")), NuGetFramework.AnyFramework);

            var diff = PackageVersionDiffer.GetVersionDiff(packageReference, new[]
            {
                new NuGetVersion("1.0.0"),
                new NuGetVersion("5.0.0"),
                new NuGetVersion("6.0.0-pre"),
            });

            Assert.Equal(new SemanticVersion(3, 0, 0), diff.Diff);
        }

        [Fact]
        public void CanReturnUnstableVersionDifferenceWithSameReleaseTag()
        {
            var packageReference = new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1-pre")), NuGetFramework.AnyFramework);

            var diff = PackageVersionDiffer.GetVersionDiff(packageReference, new[]
            {
                new NuGetVersion("1.0.0"),
                new NuGetVersion("5.0.0"),
                new NuGetVersion("6.0.0-pre"),
            });

            Assert.Equal(new SemanticVersion(4, 0, 0), diff.Diff);
        }

        [Fact]
        public void CanReturnUnstableVersionDifferenceWithDifferentReleaseTag()
        {
            var packageReference = new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1-anotherpre")), NuGetFramework.AnyFramework);

            var diff = PackageVersionDiffer.GetVersionDiff(packageReference, new[]
            {
                new NuGetVersion("1.0.0"),
                new NuGetVersion("5.0.0"),
                new NuGetVersion("6.0.0-pre"),
            });

            Assert.Equal(new SemanticVersion(3, 0, 0), diff.Diff);
        }

        [Fact]
        public void CanReturnUnstableVersionDifferenceWithNoReleaseTags()
        {
            var packageReference = new PackageReference(new PackageIdentity("AutoMapper", new NuGetVersion("2.2.1-anotherpre")), NuGetFramework.AnyFramework);

            var diff = PackageVersionDiffer.GetVersionDiff(packageReference, new[]
            {
                new NuGetVersion("1.0.0"),
                new NuGetVersion("5.0.0")
            });

            Assert.Equal(new SemanticVersion(3, 0, 0), diff.Diff);
        }
    }
}
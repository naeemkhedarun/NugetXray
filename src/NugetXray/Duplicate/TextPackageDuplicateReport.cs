using System.Linq;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class TextPackageDuplicateReport : TextReport
    {
        private readonly IOrderedEnumerable<PackageDuplicate> _results;
        private readonly bool _verbose;

        public TextPackageDuplicateReport(IOrderedEnumerable<PackageDuplicate> results, bool verbose)
        {
            _results = results;
            _verbose = verbose;
        }

        protected override void CreateReport()
        {
            var errors = 0;

            foreach (var packageDuplicate in _results)
            {
                errors++;

                var diffMessage = $"{packageDuplicate.Versions.Select(x => x.Item1).Distinct().Count()} versions";

                if (_verbose)
                {
                    WriteError($"{packageDuplicate.PackageReference.PackageIdentity.Id} | {diffMessage}");

                    Write(string.Empty);
                    foreach (var version in packageDuplicate.Versions)
                    {
                        WriteError($"  {version}");
                    }
                    Write(string.Empty);
                }
                else
                {
                    WriteError(
                        $"{packageDuplicate.PackageReference.PackageIdentity.Id.PadRight(70)} | {diffMessage.PadRight(10)}");
                }
            }
            
            Write(string.Empty);
            WriteError($"Errors:   {errors}");
            Write(string.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using NuGet.Versioning;
using System.Linq;

namespace NugetXray.Diff
{
    internal class ConsolePackageDiffReport : TextReport
    {
        private readonly IEnumerable<PackageDiffReportItem> _results;
        private readonly bool _verbose;

        private readonly string _fullTemplate = @"{0,70} | {1,10}
 | {2,3} configs
";

        private readonly string _verboseTemplate = @"{0} | -{1}
{2}

";

        public ConsolePackageDiffReport(IEnumerable<PackageDiffReportItem> results, bool verbose)
        {
            _results = results;
            _verbose = verbose;
        }

        protected override void CreateReport()
        {
            int errors = 0, warnings = 0;

            foreach (var packageDiffReport in _results)
            {
                var diffMessage = packageDiffReport.Diff.WasFoundInFeed ? packageDiffReport.Diff.Diff.ToString() : "Not found.";

                var log = _verbose
                    ? string.Format(_verboseTemplate,
                        packageDiffReport.Package.PackageIdentity, diffMessage,
                        string.Join(Environment.NewLine, packageDiffReport.Configs.Select(x => $"  {x}")))
                    : string.Format(_fullTemplate,
                        packageDiffReport.Package.PackageIdentity.ToString(),
                        diffMessage,
                        packageDiffReport.Configs.Length);

                if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 0, 0))
                {
                    WriteError(log);
                    errors++;
                }
                else if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 1, 0))
                {
                    WriteWarning(log);
                    warnings++;
                }
                else if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 0, 1))
                {
                    Write(log);
                }
            }
            Write($"Errors: {errors}\n{warnings}\n");
        }
    }
}
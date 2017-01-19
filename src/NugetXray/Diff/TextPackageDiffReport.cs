using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    internal class TextPackageDiffReport
    {
        private readonly string _fullTemplate = @"{0,-70} | {1,10} | {2,3} configs";

        private readonly string _verboseTemplate = @"{0} | -{1}
{2}

";

        public List<string> CreateReport(IEnumerable<PackageDiffReportItem> results, bool verbose)
        {
            var messages = new List<string>();

            int errors = 0, warnings = 0;

            if (!verbose)
            {
                messages.Add(string.Format(_fullTemplate, "package", "version", ""));
                messages.Add(new string('-', 71) + "+" + new string('-', 12) + "+" + new string('-', 12));
            }

            foreach (var packageDiffReport in results)
            {
                var diffMessage = packageDiffReport.Diff.WasFoundInFeed ? packageDiffReport.Diff.Diff.ToString() : "Not found.";

                var log = verbose
                    ? string.Format(_verboseTemplate,
                        packageDiffReport.Package.PackageIdentity, diffMessage,
                        string.Join(Environment.NewLine, packageDiffReport.Configs.Select(x => $"  {x}")))
                    : string.Format(_fullTemplate,
                        packageDiffReport.Package.PackageIdentity.ToString(),
                        diffMessage,
                        packageDiffReport.Configs.Length);

                if (packageDiffReport.Diff.Diff >= new SemanticVersion(1, 0, 0))
                {
                    messages.Add(log);
                    errors++;
                }
                else if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 1, 0))
                {
                    messages.Add(log);
                    warnings++;
                }
                else if (packageDiffReport.Diff.Diff > new SemanticVersion(0, 0, 0))
                {
                    messages.Add(log);
                }
            }
            messages.Add($"Errors: {errors}\n{warnings}\n");

            return messages;
        }
    }
}
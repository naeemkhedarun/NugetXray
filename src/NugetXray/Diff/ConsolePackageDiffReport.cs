using System;
using System.Collections.Generic;
using NuGet.Versioning;

namespace NugetXray
{
    internal class ConsolePackageDiffReport
    {
        private readonly IEnumerable<PackageDiffReportItem> _results;
        private readonly bool _verbose;

        public ConsolePackageDiffReport(IEnumerable<PackageDiffReportItem> results, bool verbose)
        {
            _results = results;
            _verbose = verbose;
        }

        public void Write()
        {
            var defaultColor = Console.ForegroundColor;
            int errors = 0, warnings = 0;

            foreach (var packageDiffReport in _results)
            {
                if (packageDiffReport.Diff.Diff >= new SemanticVersion(1, 0, 0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    errors++;
                }
                else if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 1, 0))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    warnings++;
                }
                else if (packageDiffReport.Diff.Diff >= new SemanticVersion(0, 0, 1))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (_verbose)
                {
                    Console.Write($"{packageDiffReport.Package.PackageIdentity} | -{packageDiffReport.Diff.Diff}");

                    Console.WriteLine();
                    foreach (var config in packageDiffReport.Configs)
                    {
                        Console.WriteLine($"  {config}");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.Write($"{packageDiffReport.Package.PackageIdentity.ToString().PadRight(70)} | -{packageDiffReport.Diff.Diff.ToString().PadRight(10)}");

                    Console.Write($" | {packageDiffReport.Configs.Length.ToString().PadRight(3)} configs");
                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = defaultColor;

            Console.WriteLine("");
            Console.WriteLine($"Errors:   {errors}");
            Console.WriteLine($"Warnings: {warnings}");
            Console.WriteLine("");
        }
    }
}
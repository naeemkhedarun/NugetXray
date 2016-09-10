using System;
using System.Linq;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class ConsolePackageDuplicateReport
    {
        private readonly IOrderedEnumerable<PackageDuplicate> _results;
        private readonly bool _verbose;

        public ConsolePackageDuplicateReport(IOrderedEnumerable<PackageDuplicate> results, bool verbose)
        {
            _results = results;
            _verbose = verbose;
        }

        public void Write()
        {
            var defaultColor = Console.ForegroundColor;
            var errors = 0;

            Console.ForegroundColor = ConsoleColor.Red;

            foreach (var packageDuplicate in _results)
            {
                errors++;

                var diffMessage = $"{packageDuplicate.Versions.Select(x => x.Item1).Distinct().Count()} versions";

                if (_verbose)
                {
                    Console.Write($"{packageDuplicate.PackageReference.PackageIdentity.Id} | {diffMessage}");

                    Console.WriteLine();
                    foreach (var version in packageDuplicate.Versions)
                    {
                        Console.WriteLine($"  {version}");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{packageDuplicate.PackageReference.PackageIdentity.Id.PadRight(70)} | {diffMessage.PadRight(10)}");
                }
            }

            Console.ForegroundColor = defaultColor;

            Console.WriteLine("");
            Console.WriteLine($"Errors:   {errors}");
            Console.WriteLine("");
        }
    }
}
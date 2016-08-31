using System;
using System.Collections.Generic;
using CommandLine;
using NugetXray.Diff;

namespace NugetXray
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<PackageDiffCommand>(args);

            var exitCode = result.MapResult(
                (PackageDiffCommand o) => o.RunAsync().Result,
                OnParseFailure);

            return exitCode;
        }

        private static int OnParseFailure(IEnumerable<Error> errors)
        {
            Console.WriteLine(errors);
            return 1;
        }
    }
}

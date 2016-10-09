using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace NugetXray
{
    public class ReportWriter
    {
        public void Write(IEnumerable<CommandResult> reports)
        {
            var defaultColor = Console.ForegroundColor;

            foreach (var commandResult in reports.Where(x => x.Report != null))
            {
                foreach (var tuple in commandResult.Report.GetReport())
                {
                    switch (tuple.Item1)
                    {
                        case LogLevel.Information:
                            Console.ForegroundColor = defaultColor;
                            break;
                        case LogLevel.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case LogLevel.Error:
                            Console.ForegroundColor = ConsoleColor.Red;

                            break;
                    }
                    Console.WriteLine(tuple.Item2);
                }
            }

            Console.ForegroundColor = defaultColor;
        }
    }
}
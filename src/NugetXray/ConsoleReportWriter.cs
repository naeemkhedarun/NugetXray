using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NugetXray
{
    public class ConsoleReportWriter : IReportWriter
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

    public interface IReportWriter
    {
        void Write(IEnumerable<CommandResult> reports);
    }

    internal class FileReportWriter : IReportWriter
    {
        private readonly string _outputFile;

        public FileReportWriter(string outputFile)
        {
            _outputFile = outputFile;
        }

        public void Write(IEnumerable<CommandResult> reports)
        {
            File.WriteAllText(_outputFile, reports.First().Report.GetReport().First().Item2);
        }
    }
}
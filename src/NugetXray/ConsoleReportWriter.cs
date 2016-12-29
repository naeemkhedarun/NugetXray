using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace NugetXray
{
    public class ConsoleReportWriter : IReportWriter
    {
        public void Write(CommandResult report)
        {
            var defaultColor = Console.ForegroundColor;

            foreach (var tuple in report.Report.GetReport())
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

            Console.ForegroundColor = defaultColor;
        }
    }

    public interface IReportWriter
    {
        void Write(CommandResult report);
    }

    internal class FileReportWriter : IReportWriter
    {
        private readonly string _outputFile;

        public FileReportWriter(string outputFile)
        {
            _outputFile = outputFile;
        }

        public void Write(CommandResult report)
        {
            File.WriteAllText(_outputFile, report.Report.GetReport().First().Item2);
        }
    }
}
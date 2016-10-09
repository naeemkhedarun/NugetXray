using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
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

    public class CommandProcessor
    {
        private readonly IEnumerable<ICommand> _handlers;

        public CommandProcessor(IEnumerable<ICommand> handlers)
        {
            _handlers = handlers;
        }

        public CommandResult[] Process(string[] args)
        {
            var result = Parser.Default.ParseArguments(args, _handlers.Select(x => x.CommandType).ToArray());

            var returnValue = 0;
            var reports = new CommandResult[0];
            result.WithParsed(o => reports = Process(o));
            result.WithNotParsed(e => returnValue = OnParseFailure(e));
            return reports;
        }

        private CommandResult[] Process(object o)
        {
            return Task.WhenAll(_handlers.Where(x => x.CanHandle(o.GetType())).Select(x => x.Execute(o))).Result;
        }

        private static int OnParseFailure(IEnumerable<Error> errors)
        {
            Console.WriteLine(errors);
            return 1;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace NugetXray
{
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

            CommandProcessorReport report = null;

            var reports = new CommandResult[0];
            result.WithParsed(o => reports = Process(o));
            result.WithNotParsed(e => report = new CommandProcessorReport(e));

            return reports.Any() ? reports : new[] {new CommandResult(report, 1, "No command run.")};
        }

        private CommandResult[] Process(object o)
        {
            var commandResults = Task.WhenAll(_handlers.Where(x => x.CanHandle(o.GetType())).Select(x => x.Execute(o))).Result;

            var command = (Command) o;

            var writers = new List<IReportWriter> {new ConsoleReportWriter()};
           
            if (!string.IsNullOrEmpty(command.OutputFile))
            {
                writers.Add(new FileReportWriter(command.OutputFile));
            }

            foreach (var reportWriter in writers)
            {
                reportWriter.Write(commandResults);
            }

            return commandResults;
        }
    }

}
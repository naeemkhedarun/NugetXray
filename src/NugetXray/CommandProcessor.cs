using System;
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

        public int Process(string[] args)
        {
            var result = Parser.Default.ParseArguments(args, _handlers.Select(x => x.CommandType).ToArray());

            int errorCode = -1;

            result.WithParsed(o =>
            {
                var validationErrors = ((ICommandValidator) o).GetErrors().ToList();
                errorCode = validationErrors.Any()
                    ? FailedProcess(validationErrors)
                    : _handlers.First(x => x.CommandType == o.GetType()).Execute(o).Result;
            }).WithNotParsed(e => errorCode = FailedProcess(e.Select(x => x.Tag.ToString())));

            return errorCode;
        }

        private static int FailedProcess(IEnumerable<string> validationErrors)
        {
            foreach (var validationError in validationErrors) Console.WriteLine(validationError);

            return -1;
        }
    }
}
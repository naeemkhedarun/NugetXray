using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NugetXray
{
    public class Program
    {
        private readonly ILogger _logger;
        private readonly CommandProcessor _commandProcessor;

        public Program(ILogger<Program> logger, CommandProcessor commandProcessor)
        {
            _logger = logger;
            _commandProcessor = commandProcessor;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<int> Run(string[] args)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var process = _commandProcessor.Process(args);
            
            Environment.Exit(process);

            return process;
        }
    }
}

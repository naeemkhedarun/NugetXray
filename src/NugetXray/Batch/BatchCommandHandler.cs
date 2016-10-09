using System;
using System.Linq;
using System.Threading.Tasks;

namespace NugetXray.Batch
{
    class BatchCommandHandler : ICommand
    {
        private readonly Type _type = typeof(BatchCommand);
        public Lazy<CommandProcessor> CommandProcessor { get; set; }

        public bool CanHandle(Type command)
        {
            return _type == command;
        }

        public async Task<CommandResult> Execute(object command)
        {
            var batchCommand = (BatchCommand) command;
            var results = (await Task.WhenAll(batchCommand.Commands.Select(x => Task.Run(() => CommandProcessor.Value.Process(x.Split(' ')))))).SelectMany(x => x).ToList();

            return new CommandResult(new BatchReport(results.Select(x => x.Report)), results.Max(x => x.Code));
        }

        public Type CommandType => _type;
    }
}
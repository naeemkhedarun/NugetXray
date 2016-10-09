using System.Collections.Generic;
using CommandLine;

namespace NugetXray.Batch
{
    [Verb("batch", HelpText = "Runs a batch of commands ")]
    public class BatchCommand
    {
        [Value(0)]
        public IEnumerable<string> Commands { get; set; }
    }
}
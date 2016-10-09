using System.Collections.Generic;

namespace NugetXray.Batch
{
    internal class BatchReport : TextReport
    {
        private readonly IEnumerable<TextReport> _results;

        public BatchReport(IEnumerable<TextReport> results)
        {
            _results = results;
        }

        protected override void CreateReport()
        {
            foreach (var commandResult in _results)
            {
                WriteMany(commandResult.GetReport());
            }
        }
    }
}
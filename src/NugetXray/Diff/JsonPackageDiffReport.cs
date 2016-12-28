using System.Collections.Generic;
using Newtonsoft.Json;

namespace NugetXray.Diff
{
    internal class JsonPackageDiffReport : TextReport
    {
        private readonly IEnumerable<PackageDiffReportItem> _results;

        public JsonPackageDiffReport(IEnumerable<PackageDiffReportItem> results)
        {
            _results = results;
        }

        protected override void CreateReport()
        {
            Write(JsonConvert.SerializeObject(_results, Formatting.Indented));
        }
    }
}
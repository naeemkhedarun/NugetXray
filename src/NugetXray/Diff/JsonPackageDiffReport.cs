using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NuGet.Versioning;

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
            Write(JsonConvert.SerializeObject(
                _results.Where(x => x.Diff.Diff > new SemanticVersion(0, 0, 0)), 
                Formatting.Indented));
        }
    }
}
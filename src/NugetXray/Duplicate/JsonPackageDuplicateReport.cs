using System.Linq;
using Newtonsoft.Json;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class JsonPackageDuplicateReport : TextReport
    {
        private readonly IOrderedEnumerable<PackageDuplicate> _results;

        public JsonPackageDuplicateReport(IOrderedEnumerable<PackageDuplicate> results)
        {
            _results = results;
        }

        protected override void CreateReport()
        {
            Write(JsonConvert.SerializeObject(_results, Formatting.Indented));
        }
    }
}
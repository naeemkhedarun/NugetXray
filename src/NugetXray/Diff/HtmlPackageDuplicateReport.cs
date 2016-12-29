using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NugetXray.Diff
{
    internal class HtmlPackageDiffReport : TextReport
    {
        private readonly IOrderedEnumerable<PackageDiffReportItem> _results;

        public HtmlPackageDiffReport(IOrderedEnumerable<PackageDiffReportItem> results)
        {
            _results = results;
        }

        protected override void CreateReport()
        {
            var data = JsonConvert.SerializeObject(_results.Where(x => x.Diff.Diff > new SemanticVersion(0, 0, 0)), Formatting.Indented);

            var templateName = "NugetXray.Diff.DiffPackages.html";
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream(templateName)))
            {
                var template = reader.ReadToEnd();
                var report = template.Replace("@@data@@", data);
                Write(report);
            }
        }
    }
}
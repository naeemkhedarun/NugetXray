using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NugetXray.Diff;

namespace NugetXray.Duplicate
{
    internal class HtmlPackageDuplicateReport : TextReport
    {
        private readonly IOrderedEnumerable<PackageDuplicate> _results;

        public HtmlPackageDuplicateReport(IOrderedEnumerable<PackageDuplicate> results)
        {
            _results = results;
        }

        protected override void CreateReport()
        {
            var data = JsonConvert.SerializeObject(_results, Formatting.Indented);

//            var template = File.ReadAllText("DuplicatePackages.html");
            foreach (var manifestResourceName in Assembly.GetEntryAssembly().GetManifestResourceNames())
            {
                var templateName = "NugetXray.Duplicate.DuplicatePackages.html";
                using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream(templateName)))
                {
                    var template = reader.ReadToEnd();
                    var report = template.Replace("@@data@@", data);
                    Write(report);
                }
            }
//
//            var assembly = Assembly.GetEntryAssembly().GetManifestResourceNames()
//            var resourceStream = assembly.GetManifestResourceStream("MyNamespace.fakedata.json");

//            var report = Reports.DuplicatePackages1.Replace("@@data@@", data);

//            Write(report);
        }
    }
}
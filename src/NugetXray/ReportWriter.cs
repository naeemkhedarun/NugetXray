using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace NugetXray
{
    internal class ReportWriter
    {
        public void WriteReport(Command command, Func<string> textReport, Func<dynamic> jsonReport,
            Func<dynamic> htmlReport)
        {
            if (string.IsNullOrEmpty(command.OutputFile))
                return;

            string report;

            switch (command.GetReportFormat())
            {
                case ReportFormat.Text:
                    report = textReport();
                    break;
                case ReportFormat.Json:
                    report = JsonConvert.SerializeObject(jsonReport(), Formatting.Indented);
                    break;
                case ReportFormat.Html:
                    var data = JsonConvert.SerializeObject(htmlReport(), Formatting.Indented);
                    var assembly = Assembly.Load(new AssemblyName("NugetXray"));
                    var templateName = assembly.GetManifestResourceNames()
                        .First(x => x.EndsWith($"{command.GetType().Name}.html"));
                    var manifestResourceStream = assembly.GetManifestResourceStream(templateName);
                    using (var reader = new StreamReader(manifestResourceStream))
                    {
                        var template = reader.ReadToEnd();
                        report = template.Replace("@@data@@", data);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command.OutputFormat), command.OutputFormat, null);
            }

            File.WriteAllText(command.OutputFile, report);
            Console.WriteLine($"Saved {command.OutputFile}.");
        }
    }
}
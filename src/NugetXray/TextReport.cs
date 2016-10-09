using System;
using System.Collections.Generic;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace NugetXray
{
    public abstract class TextReport
    {
        private readonly List<Tuple<LogLevel, string>> _lines = new List<Tuple<LogLevel, string>>();

        public void WriteError(string message)
        {
            _lines.Add(Tuple.Create(LogLevel.Error, message));
        }

        public void WriteWarning(string message)
        {
            _lines.Add(Tuple.Create(LogLevel.Warning, message));
        }

        public void Write(string message)
        {
            _lines.Add(Tuple.Create(LogLevel.Information, message));
        }

        public void WriteMany(IReadOnlyList<Tuple<LogLevel, string>> lines)
        {
            _lines.AddRange(lines);
        }

        public IReadOnlyList<Tuple<LogLevel, string>> GetReport()
        {
            CreateReport();

            return _lines;
        }

        protected abstract void CreateReport();
    }

    public class CommandProcessorReport : TextReport
    {
        private readonly IEnumerable<Error> _errors;

        public CommandProcessorReport(IEnumerable<Error> errors)
        {
            this._errors = errors;
        }

        protected override void CreateReport()
        {
            foreach (var error in _errors)
            {
                WriteError(error.Tag.ToString());
            }
        }
    }
}
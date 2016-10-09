namespace NugetXray
{
    public class CommandResult
    {
        public TextReport Report { get; set; }
        public int Code { get; set; }

        public CommandResult(TextReport report, int code)
        {
            Report = report;
            Code = code;
        }
    }
}
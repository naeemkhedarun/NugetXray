namespace NugetXray
{
    public class CommandResult
    {
        public TextReport Report { get; set; }
        public int Code { get; set; }
        public string Command { get; set; }

        public CommandResult(TextReport report, int code, string command)
        {
            Report = report;
            Code = code;
            Command = command;
        }
    }
}
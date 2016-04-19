namespace GitVersion.Command
{
    using CommandLine;

    public abstract class BaseVerb : VisitableCommandOptions
    {
        [Option('l', "logfile", Required = false, HelpText = "Path to a logfile.")]
        public string LogFilePath { get; set; }

        public bool IsConsoleLoggingEnabled()
        {
            return ShouldLogToConsole() || !string.IsNullOrWhiteSpace(LogFilePath) && LogFilePath == "console"; // this argument forces log output to console.
        }

        protected abstract bool ShouldLogToConsole();

    }
}
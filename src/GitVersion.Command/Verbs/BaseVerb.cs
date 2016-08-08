namespace GitVersion.Command
{
    using System;
    using CommandLine;

    public abstract class BaseVerb : VisitableCommandOptions
    {
        [Option('l', "logfile", Required = false, HelpText = "Path to a logfile.")]
        public string LogFilePath { get; set; }

        [Option('w', "workingdir", Required = false, HelpText = "The working directory.")]
        public string WorkingDir { get; set; }

        public bool IsConsoleLoggingEnabled()
        {
            return ShouldLogToConsole() || !string.IsNullOrWhiteSpace(LogFilePath) && LogFilePath == "console"; // this argument forces log output to console.
        }

        public string GetWorkingDirectory()
        {
            return WorkingDir ?? Environment.CurrentDirectory;
        }

        protected abstract bool ShouldLogToConsole();

    }
}
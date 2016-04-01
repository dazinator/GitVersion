namespace GitVersion.Command
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using GitVersion.Helpers;

    public class Program
    {

        public static void Main(string[] args)
        {

            // var exitCode = VerifyArgumentsAndRun();

            if (Debugger.IsAttached)
            {
                Console.ReadKey();
            }

            string invokedVerb = null;
            IVisitableOptions invokedVerbInstance = null;

            var options = new Options();
            var parser = new CommandLine.Parser(with => with.IgnoreUnknownArguments = true);

            bool parsed = parser.ParseArguments(args, options, (verb, subOptions) =>
            {
                invokedVerb = verb;
                invokedVerbInstance = (IVisitableOptions)subOptions;
            });

            StringBuilder log = new StringBuilder();
            Action<string> logAction = s => log.AppendLine(s);

            if (!parsed)
            {
                Exit(1, log, true);
                return;
            }

            var commandVisitor = new CommandVisitor(logAction);
            invokedVerbInstance.Accept(commandVisitor);

            if (!commandVisitor.Success)
            {
                Exit(1, log);
                return;
            }

            // finished normally.
            Exit(0, log);

        }

        private static void Exit(int exitCode, StringBuilder log, bool showUsage = false)
        {
            if (exitCode != 0)
            {
                // Dump log to console if we fail to complete successfully
                Console.Write(log.ToString());
            }

            if (showUsage)
            {
                var options = new Options();
                var helpText = options.GetUsage();
                Console.Write(helpText);
            }

            Environment.Exit(exitCode);
        }


    }


    public class CommandVisitor : ICommandVisitor
    {
        private Action<string> _logAction;

        //private Func<IFileSystem> _fileSystemFactory;
        private IFileSystem _fileSystem;
        private Options _options;
        private LocalRepositoryOptions _localRepoOptions;

        public CommandVisitor(Action<string> logAction, IFileSystem fileSystem)
        {
            _logAction = logAction;
            _fileSystem = fileSystem;
            Success = false;
        }

        public void Visit(Options options)
        {
            _options = options;
            SetupLogging(options);
            options.RepositoryOptions?.Accept(this);
            options.ConfigOptions?.Accept(this);
            _options = null;
        }

        public void Visit(SourceRepositoryOptions options)
        {
            options.RemoteRepositoryOptions?.Accept(this);
            options.LocalRepositoryOptions?.Accept(this);
        }

        public void Visit(ConfigOptions options)
        {
            if (options.Init)
            {
                ConfigurationProvider.Init(options.GetPathForConfigYaml(), _fileSystem, new ConsoleAdapter());
            }
        }

        public void Visit(PrintVersionOptions options)
        {

        }

        public void Visit(StampAssemblyInfoOptions options)
        {

        }

        public void Visit(MsBuildOptions options)
        {

        }

        public void Visit(SpawnExecutableOptions options)
        {

        }

        public void Visit(LocalRepositoryOptions localRepositoryOptions)
        {

        }

        public void Visit(RemoteRepositoryOptions remoteRepositoryOptions)
        {
            // clone remote repo, and then intialise local repo options form the destination path.
            throw new NotImplementedException();
            //_localRepoOptions = new LocalRepositoryOptions();
        }

        public bool Success { get; set; }


        private void SetupLogging(Options options)
        {
            var writeActions = new List<Action<string>>
            {
                s => _logAction(s)
            };

            if (options.ShouldLogToConsole())
            {
                writeActions.Add(Console.WriteLine);
            }

            if (options.LogFilePath != null && options.LogFilePath != "console")
            {
                try
                {
                    var logFileFullPath = Path.GetFullPath(options.LogFilePath);
                    var logFile = new FileInfo(logFileFullPath);

                    // NOTE: logFile.Directory will be null if the path is i.e. C:\logfile.log. @asbjornu
                    if (logFile.Directory != null)
                    {
                        logFile.Directory.Create();
                    }

                    using (logFile.CreateText())
                    {
                    }

                    writeActions.Add(x => WriteLogEntry(options, x));
                }
                catch (Exception ex)
                {
                    Logger.WriteError(String.Format("Failed to configure logging for '{0}': {1}", options.LogFilePath, ex.Message));
                }
            }

            Logger.SetLoggers(
                info => writeActions.ForEach(a => a(info)),
                warn => writeActions.ForEach(a => a(warn)),
                error => writeActions.ForEach(a => a(error)));
        }

        private void WriteLogEntry(Options options, string s)
        {
            var contents = string.Format("{0}\t\t{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s);
            File.AppendAllText(options.LogFilePath, contents);
        }
    }
}

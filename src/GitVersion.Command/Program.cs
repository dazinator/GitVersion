namespace GitVersion.Command
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;
    using GitVersion.Helpers;
    using System.Linq;

    public class Program
    {

        public static void Main(string[] args)
        {

            // var exitCode = VerifyArgumentsAndRun();

            if (Debugger.IsAttached)
            {
                Console.ReadKey();
            }

            //  string invokedVerb = null;
            // IVisitableOptions invokedVerbInstance = null;

            // var options = new Options();
            var parser = new Parser(with => with.IgnoreUnknownArguments = true);
            var parsedResult = parser.ParseArguments<ConfigureVerb, MsBuildVerb, PrintVersionVerb, SpawnExecutableVerb, StampAssemblyInfoVerb>(args);

            StringBuilder log = new StringBuilder();
            Action<string> logAction = s => log.AppendLine(s);

            if (parsedResult.Tag == ParserResultType.NotParsed)
            {
                var helpText = HelpText.AutoBuild(parsedResult);

                //  var help = new CommandLine.Text.HelpText();
                Exit(1, log, helpText);
                return;
            }


            //  parsedResult.

            //bool parsed = parser.ParseArguments(args, options, (verb, subOptions) =>
            //{
            //    invokedVerb = verb;
            //    invokedVerbInstance = (IVisitableOptions)subOptions;
            //});

            //if (!parsed)
            //{
            //    Exit(1, log, true);
            //    return;
            //}
            var fileSystem = new FileSystem();
            var commandVisitor = new CommandVisitor(logAction, fileSystem);


            // parse the local repo options.
            parsedResult
                .WithParsed<ConfigureVerb>(
                    o => { o.Accept(commandVisitor); });

            // parse the local repo options.
            parsedResult
                .WithParsed<MsBuildVerb>(
                    o => { o.Accept(commandVisitor); });

            parsedResult
                .WithParsed<PrintVersionVerb>(
                    o => { o.Accept(commandVisitor); });

            parsedResult
               .WithParsed<SpawnExecutableVerb>(
                   o => { o.Accept(commandVisitor); });

            parsedResult
             .WithParsed<StampAssemblyInfoVerb>(
                 o => { o.Accept(commandVisitor); });

            //if (!commandVisitor.Success)
            //{
            //    Exit(-1, log, false);
            //}

            //.MapResult(
            //    (LocalRepositoryOptions opts) => Tuple.Create(header(opts), reader(opts)),
            //    (RemoteRepositoryOptions opts) => Tuple.Create(header(opts), reader(opts)),
            //    _ => MakeError());


            // options.Accept(commandVisitor);

            if (!commandVisitor.Success)
            {
                Exit(1, log);
                return;
            }

            // finished normally.
            Exit(0, log);

        }

        private static void Exit(int exitCode, StringBuilder log, string showUsageText = null)
        {
            if (exitCode != 0)
            {
                // Dump log to console if we fail to complete successfully
                Console.Write(log.ToString());
            }

            if (!string.IsNullOrWhiteSpace(showUsageText))
            {
                // var options = new Options();
                // todo:
                // var helpText = options.GetUsage();
                Console.Write(showUsageText);
            }

            Environment.Exit(exitCode);
        }


    }


    public class CommandVisitor : ICommandVisitor
    {
        private Action<string> _logAction;
       
        private IFileSystem _fileSystem;
        private BaseVerb _verb;
        private GitPreparer _preparer;
        private IBuildServer _buildServer;
        private BaseRepositoryOptions _repoOptions;

        public CommandVisitor(Action<string> logAction, IFileSystem fileSystem)
        {
            _logAction = logAction;
            _fileSystem = fileSystem;
            Success = false;
        }


        public void Visit(ConfigureVerb verb)
        {
            _verb = verb;
            SetupLogging(verb);
            if (verb.Init)
            {
                ConfigurationProvider.Init(verb.GetPathForConfigYaml(), _fileSystem, new ConsoleAdapter());
                Success = true;
            }
            else
            {
                var workingDir = verb.GetWorkingDirectory();
                var configAsString = ConfigurationProvider.GetEffectiveConfigAsString(workingDir, _fileSystem);
                Logger.WriteInfo(configAsString);
                Success = true;
            }
        }

        protected virtual VersionVariables GetVersionVariables(Config overrideConfig = null)
        {

            string currentBranch = ResolveCurrentBranch(_buildServer, _repoOptions.Branch, _preparer.IsDynamicGitRepository);
            _preparer.Initialise(_buildServer != null, currentBranch);

            var core = new ExecuteCore(_fileSystem);
            var versionVariables = core.ExecuteGitVersion(_preparer, currentBranch, _repoOptions.Commit, _buildServer, overrideConfig);

            return versionVariables;

        }

        public void Visit(PrintVersionVerb verb)
        {
            _verb = verb;
            SetupLogging(verb);
            verb.RepositoryOptions.Accept(this);

            var variables = GetVersionVariables();

            if (verb.Format == OutputType.BuildServer)
            {
                foreach (var buildServer in BuildServerList.GetApplicableBuildServers())
                {
                    buildServer.WriteIntegration(Logger.WriteInfo, variables);
                }
            }
            else if (verb.Format == OutputType.Json)
            {
                switch (verb.VariableName)
                {
                    case null:
                        Logger.WriteInfo(JsonOutputFormatter.ToJson(variables));
                        break;

                    default:
                        string part;
                        if (!variables.TryGetValue(verb.VariableName, out part))
                        {
                            throw new WarningException(string.Format("'{0}' variable does not exist", verb.VariableName));
                        }
                        Console.WriteLine(part);
                        break;
                }
            }

            Success = true;

        }

        public void Visit(StampAssemblyInfoVerb verb)
        {
            _verb = verb;
            SetupLogging(verb);
            verb.RepositoryOptions.Accept(this);

            throw new NotImplementedException();
            //using (var assemblyInfoUpdate = new AssemblyInfoFileUpdate(arguments, targetPath, variables, fileSystem))
            //{
            //    var execRun = RunExecCommandIfNeeded(arguments, targetPath, variables);
            //    var msbuildRun = RunMsBuildIfNeeded(arguments, targetPath, variables);
            //    if (!execRun && !msbuildRun)
            //    {
            //        assemblyInfoUpdate.DoNotRestoreAssemblyInfo();
            //        //TODO Put warning back
            //        //if (!context.CurrentBuildServer.IsRunningInBuildAgent())
            //        //{
            //        //    Console.WriteLine("WARNING: Not running in build server and /ProjectFile or /Exec arguments not passed");
            //        //    Console.WriteLine();
            //        //    Console.WriteLine("Run GitVersion.exe /? for help");
            //        //}
            //    }
            //}

        }

        public void Visit(MsBuildVerb verb)
        {
            _verb = verb;
            SetupLogging(verb);
            verb.RepositoryOptions.Accept(this);

            throw new NotImplementedException();
        }

        public void Visit(SpawnExecutableVerb verb)
        {
            _verb = verb;
            SetupLogging(verb);
            verb.RepositoryOptions.Accept(this);

            throw new NotImplementedException();
        }

        public void Visit(LocalRepositoryOptions localRepositoryOptions)
        {
            // update git repo etc.
            var workingDir = _verb.GetWorkingDirectory();
            var noFetch = ShouldPreventFetch(localRepositoryOptions);
            _preparer = new GitPreparer(null, null, null, noFetch, workingDir);
        }

        private bool ShouldPreventFetch(BaseRepositoryOptions repoOptions)
        {
            _buildServer = BuildServerList.GetApplicableBuildServers().FirstOrDefault();
            _repoOptions = repoOptions;
            return repoOptions.NoFetch || (_buildServer != null && _buildServer.PreventFetch());
        }

        public void Visit(RemoteRepositoryOptions remoteRepositoryOptions)
        {
            // clone remote repo, and then intialise local repo options form the destination path.
            var auth = new Authentication
            {
                Username = remoteRepositoryOptions.Username,
                Password = remoteRepositoryOptions.Password
            };

            var workingDir = _verb.GetWorkingDirectory();
            var noFetch = ShouldPreventFetch(remoteRepositoryOptions);
            this._preparer = new GitPreparer(remoteRepositoryOptions.Url, remoteRepositoryOptions.DestinationDirectory, auth, noFetch, workingDir);

        }

        static string ResolveCurrentBranch(IBuildServer buildServer, string targetBranch, bool isDynamicRepo)
        {
            if (buildServer == null)
            {
                return targetBranch;
            }


            var currentBranch = buildServer.GetCurrentBranch(isDynamicRepo) ?? targetBranch;
            Logger.WriteInfo("Branch from build environment: " + currentBranch);
            return currentBranch;
        }

        public bool Success { get; set; }

        private void SetupLogging(BaseVerb options)
        {
            var writeActions = new List<Action<string>>
            {
                s => _logAction(s)
            };

            if (options.IsConsoleLoggingEnabled())
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

        private void WriteLogEntry(BaseVerb options, string s)
        {
            var contents = string.Format("{0}\t\t{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s);
            File.AppendAllText(options.LogFilePath, contents);
        }
    }
}

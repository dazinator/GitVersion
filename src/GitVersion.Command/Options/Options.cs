namespace GitVersion.Command
{
    using CommandLine;
    using CommandLine.Text;

    public class Options : VisitableCommandOptions
    {

        [VerbOption("use", Required = true, HelpText = "specify a repository to use.")]
        public SourceRepositoryOptions RepositoryOptions { get; set; }

        [VerbOption("config", MutuallyExclusiveSet = "admin-mode", HelpText = "Initiliase or output gitversion configuration information.")]
        public ConfigOptions ConfigOptions { get; set; }

        [VerbOption("print", MutuallyExclusiveSet = "version-mode", HelpText = "Print version information to the console.")]
        public PrintVersionOptions PrintVersionOptions { get; set; }

        [VerbOption("stamp-assemblyinfo", MutuallyExclusiveSet = "version-mode", HelpText = "stamps the version number within assembly info files.")]
        public StampAssemblyInfoOptions StampAssemblyInfoOptions { get; set; }

        [VerbOption("msbuild", MutuallyExclusiveSet = "version-mode", HelpText = "builds an msbuild file, gitversion variables will be passed in as msbuild properties.")]
        public MsBuildOptions MsBuildOptions { get; set; }

        [VerbOption("spawn", MutuallyExclusiveSet = "version-mode", HelpText = "spawns the specified executable, exposing version variables to the process as environment variables.")]
        public SpawnExecutableOptions SpawnOptions { get; set; }

        [Option('l', "logfile", Required = false, HelpText = "Path to a logfile.")]
        public string LogFilePath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
            // help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: dnx gitversion.command use localrepo print --format Json stamp-assemblyinfo --file GlobalAssemblyInfo.cs");
            help.AddOptions(this);
            return help;
        }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public bool ShouldLogToConsole()
        {
            return
                PrintVersionOptions != null && PrintVersionOptions.Format == OutputType.BuildServer
                || LogFilePath == "console"
                || ConfigOptions != null && ConfigOptions.Init;
        }
    }
}
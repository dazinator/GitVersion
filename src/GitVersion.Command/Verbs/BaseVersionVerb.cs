using CommandLine;

namespace GitVersion.Command
{
    public abstract class BaseVersionVerb : BaseVerb
    {

        public BaseVersionVerb()
        {
            RemoteRepositoryOptions = new RemoteRepositoryOptions();
            LocalRepositoryOptions = new LocalRepositoryOptions();
        }

        [Option("use-remote-repo")]
        // [VerbOption("repo", HelpText = "the repository to inspect.")]
        public RemoteRepositoryOptions RemoteRepositoryOptions { get; set; }

        [Option("use-local-repo")]
        // [VerbOption("localrepo", MutuallyExclusiveSet = "local", HelpText = "the repository is a local one.")]
        public LocalRepositoryOptions LocalRepositoryOptions { get; set; }



        public BaseRepositoryOptions RepositoryOptions
        {
            get
            {
                if (RemoteRepositoryOptions != null)
                {
                    return RemoteRepositoryOptions;
                }
                return LocalRepositoryOptions;
            }
        }



        //[VerbOption("print", MutuallyExclusiveSet = "version-mode", HelpText = "Print version information to the console.")]
        //public PrintVersionOptions PrintVersionOptions { get; set; }

        /// <summary>
        /// By Default, we don't allow GitVersion log output to the console, because this may intefere with the output from commands 
        /// when in interactive mode. For example, if a user executes the Print verb in the console, they expect to see only the output from that
        /// command which is the version info, in the format specified, not also general logging details.
        /// Certain verbs may override this value, to enable log output to be written to the console, or if a log argument of "console" is specified
        /// this may still force log output to the console irrespective if this setting. This is why it's a "Should" method and not a "Must" method :)
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldLogToConsole()
        {
            return false;
        }
    }
}
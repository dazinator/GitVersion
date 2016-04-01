namespace GitVersion.Command
{
    using CommandLine;

    public abstract class RepositoryOptions : VisitableCommandOptions
    {
        [Option('b', "branch", Required = false, DefaultValue = "master", HelpText = "The branch to calculate the version number for.")]
        public string Branch { get; set; }

        [Option('c', "commit", Required = false, HelpText = "The commit id to calculate the version number for. Defaults to latest available commit on the branch.")]
        public string Commit { get; set; }

        [Option('n', "nofetch", Required = false, HelpText = "Disables 'git fetch' during version calculation. May cause an unexpected version number to be output.")]
        public bool NoFetch { get; set; }

    }
}
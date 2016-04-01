namespace GitVersion.Command
{
    using CommandLine;

    public class SourceRepositoryOptions : VisitableCommandOptions
    {
        [VerbOption("remoterepo", MutuallyExclusiveSet = "remote", HelpText = "the repository is a remote one that must be cloned from a given loation.")]
        public RemoteRepositoryOptions RemoteRepositoryOptions { get; set; }

        [VerbOption("localrepo", MutuallyExclusiveSet = "local", HelpText = "the repository is a local one.")]
        public LocalRepositoryOptions LocalRepositoryOptions { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
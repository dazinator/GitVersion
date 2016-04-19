namespace GitVersion.Command
{
    using CommandLine;

    [Verb("use-remote-repo", HelpText = "Use a remote repository to procude the version information, by cloning it from the given url, using the given credentials, to a local directory.")]
    public class RemoteRepositoryOptions : BaseRepositoryOptions
    {

        [Option('r', "url", Required = false, HelpText = "The url of the repository.")]
        public string Url { get; set; }

        [Option('u', "username", Required = false, HelpText = "The username to authenticate with. By default, gitversion will use the value of the GITVERSION_REMOTE_USERNAME environment variable.")]
        public string Username { get; set; }

        [Option('p', "password", Required = false, HelpText = "The password to authenticate with. By default, gitversion will use the value of the GITVERSION_REMOTE_PASSWORD environment variable.")]
        public string Password { get; set; }

        [Option('d', "destination", Required = false, HelpText = "The directory where the repository will be cloned to. Defaults to %tmp% directory.")]
        public string DestinationDirectory { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
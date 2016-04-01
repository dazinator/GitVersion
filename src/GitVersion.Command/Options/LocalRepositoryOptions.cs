namespace GitVersion.Command
{
    using System;
    using CommandLine;

    public class LocalRepositoryOptions : RepositoryOptions
    {
        public LocalRepositoryOptions()
        {
            this.FilePath = Environment.CurrentDirectory;
        }

        [Option('f', "filepath", Required = false, HelpText = "The path of the .git directory. Defaults to current directory.")]
        public string FilePath { get; set; }


        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
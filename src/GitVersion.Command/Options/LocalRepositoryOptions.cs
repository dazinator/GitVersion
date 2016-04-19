namespace GitVersion.Command
{
    using System;
    using CommandLine;

   
    public class LocalRepositoryOptions : BaseRepositoryOptions
    {

        public LocalRepositoryOptions(string[] args)
        {
            this.FilePath = Environment.CurrentDirectory;
        }

        public LocalRepositoryOptions(string args)
        {
            this.FilePath = Environment.CurrentDirectory;
        }

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
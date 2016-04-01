namespace GitVersion.Command
{
    using CommandLine;

    public class SpawnExecutableOptions : VisitableCommandOptions
    {
        [Option('e', "executable", Required = true, HelpText = "The executable that GitVersion should run, whilst making version variables available as environment variables.")]
        public string ProjectFile { get; set; }

        [Option('a', "args", Required = false, HelpText = "Additional arguments to pass to the executable.")]
        public string Arguments { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
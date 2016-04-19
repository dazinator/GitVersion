namespace GitVersion.Command
{
    using CommandLine;


    /// <summary>
    /// Options for printing the version information to the console.
    /// </summary>
    [Verb("print", HelpText = "Print version information to the console.")]
    public class PrintVersionVerb : BaseVersionVerb
    {

        [Option('t', "format", Required = false, Default = OutputType.Json, HelpText = "Determines the format of the output to the console.")]
        public OutputType Format { get; set; }

        [Option('v', "variable", Required = false, HelpText = "Will limit the output to the specified gitversion variable.")]
        public string VariableName { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override bool ShouldLogToConsole()
        {
            return Format == OutputType.BuildServer;
        }
    }
}
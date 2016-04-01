namespace GitVersion.Command
{
    using CommandLine;

    /// <summary>
    /// Options for printing the version information to the console.
    /// </summary>
    public class PrintVersionOptions : VisitableCommandOptions
    {

        [Option('f', "format", Required = false, DefaultValue = OutputType.Json, HelpText = "Determines the format of the output to the console.")]
        public OutputType Format { get; set; }

        [Option('v', "variable", Required = false, HelpText = "Will limit the output to the specified gitversion variable.")]
        public string VariableName { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
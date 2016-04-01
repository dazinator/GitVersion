namespace GitVersion.Command
{
    using CommandLine;

    public class MsBuildOptions : VisitableCommandOptions
    {
        [Option('p', "projectfile", Required = true, HelpText = "The project file to build with msbuild.")]
        public string ProjectFile { get; set; }

        [Option('a', "args", Required = false, HelpText = "Additional arguments to pass to MsBuild.")]
        public string Arguments { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
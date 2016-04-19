namespace GitVersion.Command
{
    using CommandLine;

    [Verb("msbuild", HelpText = "builds an msbuild file, gitversion variables will be passed in as msbuild properties.")]

    public class MsBuildVerb : BaseVersionVerb
    {
        [Option('m', "project-file", Required = true, HelpText = "The project file to build with msbuild.")]
        public string ProjectFile { get; set; }

        [Option('a', "args", Required = false, HelpText = "Additional arguments to pass to MsBuild.")]
        public string Arguments { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
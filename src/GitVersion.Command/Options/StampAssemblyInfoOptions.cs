namespace GitVersion.Command
{
    using CommandLine;

    public class StampAssemblyInfoOptions : VisitableCommandOptions
    {

        [Option('f', "file", Required = false, HelpText = "The path or name of an assembly info file to update. If this argument is not specified, then all assembly info files will be updated.")]
        public string Filename { get; set; }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
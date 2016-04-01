namespace GitVersion.Command
{
    public abstract class VisitableCommandOptions : IVisitableOptions
    {
        public abstract void Accept(ICommandVisitor visitor);
    }
}
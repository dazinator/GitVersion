namespace GitVersion.Command
{
    public interface IVisitableOptions
    {
        void Accept(ICommandVisitor visitor);
    }
}
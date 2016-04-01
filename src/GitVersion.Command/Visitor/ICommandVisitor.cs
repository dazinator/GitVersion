namespace GitVersion.Command
{
    public interface ICommandVisitor
    {
        void Visit(SourceRepositoryOptions options);

        void Visit(ConfigOptions options);

        void Visit(PrintVersionOptions options);

        void Visit(StampAssemblyInfoOptions options);

        void Visit(MsBuildOptions options);

        void Visit(SpawnExecutableOptions options);

        void Visit(Options options);
        void Visit(LocalRepositoryOptions localRepositoryOptions);
        void Visit(RemoteRepositoryOptions remoteRepositoryOptions);
    }
}
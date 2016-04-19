namespace GitVersion.Command
{
    public interface ICommandVisitor
    {
        // void Visit(InspectRepositoryOptions options);

       // void Visit(BaseVersionVerb options);

        void Visit(ConfigureVerb verb);

        void Visit(PrintVersionVerb verb);

        void Visit(StampAssemblyInfoVerb verb);

        void Visit(MsBuildVerb verb);

        void Visit(SpawnExecutableVerb verb);


        void Visit(LocalRepositoryOptions localRepositoryOptions);
        void Visit(RemoteRepositoryOptions remoteRepositoryOptions);
       // void Visit(LoggingOptions loggingOptions);
      //  void Visit(BaseInspectionOptions remoteRepositoryOptions);
    }
}
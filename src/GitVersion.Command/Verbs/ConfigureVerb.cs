namespace GitVersion.Command
{
    using System;
    using System.IO;
    using CommandLine;

    [Verb("config", HelpText = "Initiliase or output gitversion configuration information.")]
    public class ConfigureVerb : BaseVerb
    {
        [Option('o', "output", SetName = "output-mode", Required = false, Default = ConfigOutputType.Yaml, HelpText = "Determines the format of the output to the console.")]
        public ConfigOutputType Format { get; set; }

        [Option('i', "init", SetName = "init-mode", Required = false, HelpText = "Initialise gitversion configuration utility. Guides you through creating a config yaml file.")]
        public bool Init { get; set; }

        //[Option('w', "workingdir", SetName = "init-mode", Required = false, HelpText = "The working directory to store your configuration. Will use environment current directory by default.")]
        //public string workingdirectory { get; set; }

        public string GetPathForConfigYaml()
        {
            var workingDir = this.GetWorkingDirectory();
            if (string.IsNullOrWhiteSpace(workingDir))
            {
                return Environment.CurrentDirectory;
            }

            if (Path.IsPathRooted(workingDir))
            {
                return workingDir;
            }

            return Path.GetFullPath(workingDir);
        }

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override bool ShouldLogToConsole()
        {
             return Init;
        }
    }


   
}

//}
using System;
using LightInject;
using NugetXray.Diff;
using NugetXray.Duplicate;

namespace NugetXray
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var container = new ServiceContainer();
            container.SetDefaultLifetime<PerContainerLifetime>();
            container.Register<CommandProcessor>();
            container.Register<PackageDiffCommandHandler>();
            container.Register<PackageDuplicateCommandHandler>();
            container.Register<CachedPackageReader>();

            var process = container.GetInstance<CommandProcessor>().Process(args);

            Environment.Exit(process);

            return process;
        }
    }
}

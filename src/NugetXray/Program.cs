using System.Linq;
using LightInject;
using NugetXray.Batch;
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
            container.Register<BatchCommandHandler>();
            container.Register<CachedPackageReader>();

            var process = container.GetInstance<CommandProcessor>().Process(args);
            new ReportWriter().Write(process);  
            return process.Max(x => x.Code);
        }
    }
}

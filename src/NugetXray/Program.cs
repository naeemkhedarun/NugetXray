using System;
using System.Linq;
using LightInject;
using Microsoft.Extensions.Logging;
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
            container.Register(factory =>
            {
                var loggerFactory = new LoggerFactory();
                loggerFactory.AddConsole();
                return loggerFactory.CreateLogger("Default");
            });
            var process = container.GetInstance<CommandProcessor>().Process(args);
            new ReportWriter().Write(process);  
            Console.ReadKey();
            return process.Max(x => x.Code);
        }
    }
}

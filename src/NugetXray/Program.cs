using System;
using System.Diagnostics;
using System.Linq;
using CommandLine;
using LightInject;
using Newtonsoft.Json;
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
            
            return process.Max(x => x.Code);
        }
    }
}

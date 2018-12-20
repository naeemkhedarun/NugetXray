using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NugetXray.Diff;
using NugetXray.Duplicate;

namespace NugetXray
{
    public static class Startup
    {
        public static async Task Main(string[] args)
        {
            var program = BuildProgram(args);
            await program.Run(args);

#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<Program>();

            services.AddSingleton<CommandProcessor>();
            services.AddSingleton<CachedPackageReader>();
            services.AddSingleton<ICommand, PackageDiffCommandHandler>();
            services.AddSingleton<ICommand, PackageDuplicateCommandHandler>();
        }

        private static void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
        }

        private static Program BuildProgram(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            ConfigureLogging(loggerFactory);

            return serviceProvider.GetRequiredService<Program>();
        }
    }
}
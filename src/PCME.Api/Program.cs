using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PCME.Api.Infrastructure;
using PCME.Infrastructure;
using PCME.CustomWebHost;
using Microsoft.Extensions.Options;

namespace PCME.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<ApplicationDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var settings = services.GetService<IOptions<ApplicationSettings>>();
                    //var logger = services.GetService<ILogger<OrderingContextSeed>>();

                    new ApplicationContextSeed()
                        .SeedAsync(context, env,settings)
                        .Wait();
                })
                //.MigrateDbContext<IntegrationEventLogContext>((_, __) => { })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

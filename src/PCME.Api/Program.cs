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
using PCME.Api.Infrastructure.AutoMapperMapping;
using PCME.MOPDB;

namespace PCME.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            BuildWebHost(args)
            #region 启用/关闭导入数据库
                //            .MigrateDbContext<ApplicationDbContext>((context, services) =>
                //            {
                //                var env = services.GetService<IHostingEnvironment>();
                //                var settings = services.GetService<IOptions<ApplicationSettings>>();
                //                var mopdbcontext = services.GetService<MOPDBContext>();
                //                //var logger = services.GetService<ILogger<OrderingContextSeed>>();

                //                try
                //	{
                //		new ApplicationContextSeed()
                //			.SeedAsync(context, mopdbcontext, env, settings)
                //			.Wait();
                //	}
                //	catch (Exception ex)
                //	{
                //		throw new Exception(ex.Message);
                //	}
                //})

                //.MigrateDbContext<IntegrationEventLogContext>((_, __) => { })
            #endregion
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

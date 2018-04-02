using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Infrastructure;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure
{

    public class ApplicationContextSeed
    {
        private Policy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        //logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }

        private IEnumerable<WorkUnitNature> GetPredefinedWorkUnitNature()
        {
            return new List<WorkUnitNature>()
            {
                WorkUnitNature.JgUnit,
                WorkUnitNature.SyUnit,
                WorkUnitNature.Company
            };
        }
        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env, IOptions<ApplicationSettings> settings)
        {
            var policy = CreatePolicy(nameof(ApplicationContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                using (context)
                {
                    context.Database.Migrate();

                    if (!context.WorkUnitNature.Any())
                    {
                        context.WorkUnitNature.AddRange(GetPredefinedWorkUnitNature());

                        await context.SaveChangesAsync();
                    }

                    //if (!context.OrderStatus.Any())
                    //{
                    //    context.OrderStatus.AddRange(useCustomizationData
                    //                            ? GetOrderStatusFromFile(contentRootPath, logger)
                    //                            : GetPredefinedOrderStatus());
                    //}

                    await context.SaveChangesAsync();
                }
            });
            Console.Write("seed");
        }
    }
}

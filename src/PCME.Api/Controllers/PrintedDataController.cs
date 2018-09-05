using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PCME.Domain.AggregatesModel.CertificateAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PrintedData")]
    public class PrintedDataController:Controller
    {
        private readonly ApplicationDbContext dbContext;
        public PrintedDataController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Save([FromBody]JObject jObject,int type)
        {
            int studentid = (int)jObject["studentid"];
            var isexists = dbContext.PrintedData.Where(c => c.StudentId == studentid);
            bool insert = true;
            if (!isexists.Any())
            {
                insert = true;
            }
            var time = isexists.OrderByDescending(c => c.CreateTime).FirstOrDefault().CreateTime;
            if (DateTime.Now < time.AddDays(30) && DateTime.Now > time.AddDays(-30))
            {
                insert = false;
            }

            if (insert)
            {
                PrintedData printedData = new PrintedData(studentid, jObject.ToString(), CertificateCategory.From(type), DateTime.Now);
                dbContext.PrintedData.Add(printedData);
                dbContext.SaveChangesAsync();
            }
            return Ok(1);
        }
    }
}

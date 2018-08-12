using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.TestAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/testlibrary")]
    public class TestLibraryController:Controller
    {
        private readonly TestDBContext testContext;
        private readonly ApplicationDbContext context;
        public TestLibraryController(TestDBContext testContext,ApplicationDbContext context)
        {
            this.testContext = testContext;
            this.context = context;
        }
        [Route("readlibrary")]
        [HttpPost]
        public IActionResult ReadLibrary(int examsubjectid, string title)
        {
            var examSubject = context.ExamSubjects.Find(examsubjectid);

            var testconfig = testContext.TestConfig.Where(c => c.CategoryCode == examSubject.Code && c.Title == title)
                .Include(c=>c.TestPaper)
                .FirstOrDefault();


            var query = from c in testContext.TestLibrary
                        select c;

            List<TestLibrary> testlibrary = new List<TestLibrary>();

            foreach (var item in testconfig.TestPaper)
            {
                var temp = query.Where(c => c.TestTypeId == item.TestTypeId).OrderBy(x=>Guid.NewGuid()).Take(item.DisplayCount).ToList();
                foreach (var tmp in temp)
                {
                    tmp.score = item.Score;
                    tmp.orderby = item.OrderBy;
                }
                testlibrary.AddRange(temp);
            }
            var orderbyitem = testlibrary.OrderBy(c => c.orderby);
            var result = testlibrary.Select((c,indexer) => new Dictionary<string, object>
            {
                {"indexer",indexer+1},
                {"Id",c.Id},
                {"Answer",c.Answer},
                {"CategoryCode",c.CategoryCode},
                {"IsHomeWork",c.IsHomeWork},
                {"SelectItem",c.SelectItem},
                {"TestTypeId",c.TestTypeId},
                {"Topic",c.Topic},
                {"Score",c.score},
                {"_answer",c.answer}
            });
            return Ok(new { success=true, data = result });
        }

        [Route("save")]
        [HttpPost]
        public IActionResult Save([FromBody]string s) {
            return Ok(new { success = true,data=s });
        }
    }
}

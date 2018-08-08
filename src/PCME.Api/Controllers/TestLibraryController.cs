using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public TestLibraryController(TestDBContext testContext)
        {
            this.testContext = testContext;
        }
        [Route("readlibrary")]
        public IActionResult ReadLibrary(string categorycode, string title)
        {
            var testconfig = testContext.TestConfig.Where(c => c.CategoryCode == categorycode && c.Title == title)
                .Include(c=>c.TestPaper)
                .FirstOrDefault();


            var query = from c in testContext.TestLibrary
                        select c;

            List<TestLibrary> testlibrary = new List<TestLibrary>();

            foreach (var item in testconfig.TestPaper)
            {
                var temp = query.Where(c => c.TestTypeId == item.TestTypeId).OrderByNewId().Take(item.DisplayCount).ToList();
                foreach (var tmp in temp)
                {
                    tmp.score = item.Score;
                    tmp.orderby = item.OrderBy;
                }
                testlibrary.AddRange(temp);
            }
            var orderbyitem = testlibrary.OrderBy(c => c.orderby);
            var result = testlibrary.Select(c => new Dictionary<string, object>
            {
                {"Id",c.Id},
                {"Answer",c.Answer},
                {"CategoryCode",c.CategoryCode},
                {"IsHomeWork",c.IsHomeWork},
                {"SelectItem",c.SelectItem},
                {"TestTypeId",c.TestTypeId},
                {"Topic",c.Topic},
                {"Score",c.score}
            });
            return Ok(new { success=true, data = result });
        }
    }
}

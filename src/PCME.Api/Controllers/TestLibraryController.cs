using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.ExamResultAggregates;
using PCME.Domain.AggregatesModel.HomeWorkAggregates;
using PCME.Domain.AggregatesModel.TestAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            //System.Threading.Thread.Sleep(10000);
            var examSubject = context.ExamSubjects.Find(examsubjectid);

            var testconfig = testContext.TestConfig.Where(c => c.CategoryCode == examSubject.Code && c.Title == title)
                .Include(c=>c.TestPaper)
                .FirstOrDefault();

            if (testconfig == null)
            {
                return BadRequest();
            }


            var query = from c in testContext.TestLibrary
                        where c.CategoryCode == examSubject.Code
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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Save([FromBody]JObject obj) {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            var score = (float)obj["score"];
            var examsubjectid = (int)obj["examsubject"];
            var student = await context.Students.FindAsync(studentid);
            var examSubject = await context.ExamSubjects.FindAsync(examsubjectid);
            var testconfig = await testContext.TestConfig.Where(c => c.CategoryCode == examSubject.Code && c.Title == "作业")
                .Include(c => c.TestPaper)
                .FirstOrDefaultAsync();

            var curResult = testContext.HomeWorkResult.Where(c => c.StudentId == studentid && c.CategoryCode == examSubject.Code);
            if (curResult.Count() != 0) {
                if (curResult.Count() > testconfig.Ctr-1)
                {
                    return Ok(new { success = false, message = "已经完成" + testconfig.Ctr.ToString() + "次作业，不用再做" });
                }
            }

            var saveresult = new HomeWorkResult(studentid, score, examSubject.Code, DateTime.Now, DateTime.Now);
            testContext.Add(saveresult);
            await testContext.SaveChangesAsync();

            return Ok(new { success = true,data=new { score,examsubjectid,ctr = curResult.Count()},message="交卷成功" });
        }

        [Route("savecs")]
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SaveCS([FromBody]JObject obj)
        {
            var ticketid = (int)obj["ticketid"];
            var score = (float)obj["score"];
            var examsubjectid = (int)obj["examsubject"];
            var ticket = await context.AdmissionTicketCS.FindAsync(ticketid);
            var student = await context.Students.FindAsync(ticket.StudentId);
            var examSubject = await context.ExamSubjects.FindAsync(examsubjectid);


            if (ticket.PostPaperTime != null)
            {
                return Ok(new { success = false, message = "已经交卷，不用重复提交" });
            }

            //var examresult = testContext.ExamResult.Where(c => c.StudentId == student.Id && c.ExamSubjectId == examsubjectid);
            //if (examresult.Any())
            //{
            //    return Ok(new { success = false, message = "您已经考过本科目了，请联系管理员" });
            //}

            ExamResult examresultadd = new ExamResult(ticket.Num, ticket.Id, ticket.StudentId, ticket.ExamSubjectId, score, DateTime.Now, false);
            await testContext.ExamResult.AddAsync(examresultadd);
            await testContext.SaveChangesAsync();

            ticket.PostPaper();
            context.AdmissionTicketCS.Update(ticket);
            await context.SaveChangesAsync();

            return Ok(new { success = true, data = new { score, examsubjectid, ctr = 0 }, message = "交卷成功" });
        }
    }
}

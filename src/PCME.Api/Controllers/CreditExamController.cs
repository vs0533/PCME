using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    /// <summary>
    /// 科研/发明类
    /// </summary>
    [Produces("application/json")]
    [Route("api/CreditExam")]
    public class CreditExamController : Controller
    {
        public readonly ApplicationDbContext context;
        public CreditExamController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var creditExamQuery = from creditexam in context.CreditExams
                                  join examsubject in context.ExamSubjects on creditexam.SubjectId equals examsubject.Id into left1
                                  from examsubject in left1.DefaultIfEmpty()
                                  join students in context.Students on creditexam.StudentId equals students.Id into left2
                                  from students in left2.DefaultIfEmpty()
                                  where students.Id == studentId
                                  select new { creditexam, examsubject };


            creditExamQuery = creditExamQuery
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = creditExamQuery.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.creditexam.Id},
                {"creditexam.AdmissionTicketNum",c.creditexam.AdmissionTicketNum},
                {"creditexam.CreateTime",c.creditexam.CreateTime},
                {"creditexam.Credit",c.creditexam.Credit},
                {"examsubject.Id",c.examsubject.Id},
                {"examsubject.Name",c.examsubject.Name}
            });
            var total = creditExamQuery.Count();
            return Ok(new { total, data = result });
        }
    }
}

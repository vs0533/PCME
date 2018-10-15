using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AdmissionTicketAggregates;
using PCME.Domain.AggregatesModel.AdmissionTicketLogAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdmissionTicketCS")]
    public class AdmissionTicketCSController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly TestDBContext testcontext;
        public AdmissionTicketCSController(ApplicationDbContext context, TestDBContext testcontext)
        {
            this.context = context;
            this.testcontext = testcontext;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var admissionTicketcs = from admissionticketcs in context.AdmissionTicketCS
                                  join examsubject in context.ExamSubjects on admissionticketcs.ExamSubjectId equals examsubject.Id into left1
                                  from examsubject in left1.DefaultIfEmpty()
                                  join signup in context.SignUp on admissionticketcs.SignUpId equals signup.Id into left4
                                  from signup in left4.DefaultIfEmpty()
                                  join trainingcenter in context.TrainingCenter on signup.TrainingCenterId equals trainingcenter.Id into left3
                                  from trainingcenter in left3.DefaultIfEmpty()
                                  where admissionticketcs.StudentId == studentId
                                  select new {  admissionticketcs, examsubject, trainingcenter, signup };

            admissionTicketcs = admissionTicketcs
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = admissionTicketcs.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.admissionticketcs.Id},
                { "admissionticket.Num",c.admissionticketcs.Num},
                { "admissionticket.LoginTime",c.admissionticketcs.LoginTime},
                { "admissionticket.PostPaperTime",c.admissionticketcs.PostPaperTime},
                { "examsubject.Id",c.examsubject.Id},
                { "examsubject.Name",c.examsubject.Name},
                { "trainingcenter.Id",c.trainingcenter.Id},
                { "trainingcenter.Name",c.trainingcenter.Name},
                { "admissionticket.CreateTime",c.admissionticketcs.CreateTime},
                { "signup.CreateTime",c.signup.CreateTime}
            });
            var total = admissionTicketcs.Count();
            return Ok(new { total, data = result });
        }
        public class AdmissionTicketCSDTO {
            public int Id { get; set; }
            [JsonProperty("examsubject.Id")]
            public int ExamSubjectId { get; set; }
            public int SignUpId { get; set; }
            public void SetId(int id) {
                Id = id;
            }
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Student")]
        public IActionResult Post([FromBody]AdmissionTicketCSDTO command, string opertype)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            if (opertype == "new")
            {
                command.SetId(0);
            }
            
            var examsubject = context.ExamSubjects.Find(command.ExamSubjectId);
            var admissionticketcs = context.AdmissionTicketCS.Where(c => c.StudentId == studentId && c.ExamSubjectId == command.ExamSubjectId).FirstOrDefault();

            if (admissionticketcs != null && admissionticketcs.PostPaperTime == null)
            {
                return Ok(new { success = false, message = "该科目存在一个未使用的准考证，请不要重复生成准考证" });
            }
            if (admissionticketcs != null)
            {
                var homework = testcontext.HomeWorkResult.Where(c => c.StudentId == studentId && c.CategoryCode == examsubject.Code).ToList();
                var examresult = testcontext.ExamResult.Where(c => c.ExamSubjectId == admissionticketcs.ExamSubjectId && c.StudentId == studentId).FirstOrDefault();
                var homeworkscore = homework.Any() != true ? 0 : homework.Sum(c => c.Score);
                var examscore = examresult == null ? 0 : examresult.Score;
                var score = homeworkscore + examscore;
                if (examscore > 0 && score >= 60)
                {
                    return Ok(new { success = false, message = "准考证已经合格，一个工作日后计入学分。" });
                }
            }
            var count = context.AdmissionTicketCS.Count();
            count = count + 1;


            //string num = command.ExamSubjectId + plan.Num + (plancount + 1).ToString().PadLeft(3, '0');
            string num = examsubject.Code.Trim() + DateTime.Now.ToString("yyMMdd")+ count.ToString().PadLeft(5, '0');

            AdmissionTicketCS ticket = new AdmissionTicketCS(num, studentId, command.SignUpId, command.ExamSubjectId
                , null, null, DateTime.Now);

            var numisExists = context.AdmissionTicketCS.Where(c => c.Num == num).Any();
            if (numisExists)
            {
                return Ok(new { success = false, message = "存在相同的准考证号，生成失败" });
            }

            //更改报名标记 保存准考证
            var signup = context.SignUp.Where(c => c.Id == command.SignUpId).FirstOrDefault();
            signup.TicketChangeCreate();
            context.AdmissionTicketCS.Add(ticket);
            context.SignUp.Update(signup);
            
            context.SaveChanges();
            return Ok(new { success = true, message = "生成成功！" });
        }
    }
}

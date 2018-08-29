using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Exam.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdmissionTickets")]
    public class AdmissionTicketsController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly TestDBContext testContext;
        public AdmissionTicketsController(ApplicationDbContext context, TestDBContext testContext)
        {
            this.context = context;
            this.testContext = testContext;
        }
        [HttpPost]
        [Route("signin")]
        [Authorize(Roles="RoomAccount")]
        public IActionResult SignIn(string ticket) {

            var roomaccountid = int.Parse(User.FindFirstValue("AccountId"));
            var roomaccount = context.ExaminationRoomAccount.Find(roomaccountid);
            if (roomaccount == null)
            {
                return Ok(new { success = false, message = "超时，请重新登录" });
            }
            var admissionticket = context.AdmissionTickets.Where(c => c.Num == ticket).FirstOrDefault();

            var roomplan = context.ExaminationRoomPlans.Where(c => c.Id == admissionticket.ExaminationRoomPlanId).FirstOrDefault();
            if (DateTime.Now < roomplan.SignInTime.AddMinutes(-30) || DateTime.Now > roomplan.SignInTime.AddMinutes(20))
            {
                return Ok(new { success = false, message = string.Format("未在签到时间内不允许签到，签到时间为{0}-{1}",roomplan.SignInTime.AddMinutes(-30), roomplan.SignInTime.AddMinutes(20)) });
            }

            if (admissionticket == null)
            {
                return Ok(new { success = false, message = "未找到对应的准考证号" });
            }
            if (admissionticket.SignInTime != null)
            {
                return Ok(new { success = false, message = "该人员已经签到，不允许重复签到。" });
            }
            admissionticket.SignIn();
            context.AdmissionTickets.Update(admissionticket);
            context.SaveChanges();
            return Ok(new { success = true, message = "签到成功" });
        }

        [HttpPost]
        [Route("readticketinfo")]
        [Authorize(Roles = "Exam")]
        public IActionResult ReadTicketInfo(int ticketid)
        {
            var ticket = (from admissiontickets in context.AdmissionTickets
                         join student in context.Students on admissiontickets.StudentId equals student.Id
                         where admissiontickets.Id == ticketid
                         select new {
                             admissiontickets.Id,
                             admissiontickets.Num,
                             admissiontickets.LoginTime,
                             admissiontickets.PostPaperTime,
                             admissiontickets.StudentId,
                             admissiontickets.ExamSubjectId,
                             admissiontickets.ExaminationRoomPlanId,
                             admissiontickets.ExaminationRoomId,
                             student.IDCard
                         }).FirstOrDefault();
            //var examsubjectcode =  context.ExamSubjects.Find(ticket.ExamSubjectId).Code;
            //var minute = testContext.TestConfig.Where(c => c.CategoryCode == examsubjectcode && c.Title == "考试").FirstOrDefault();
            var examationroomplant = context.ExaminationRoomPlans.Find(ticket.ExaminationRoomPlanId);
            var plantendtime = examationroomplant.SignInTime.AddMinutes(60);
            var logendtime = DateTime.Now.AddMinutes(60);
            var starttime = DateTime.Now;
            var endtime = logendtime < plantendtime ? logendtime : plantendtime;
            if (endtime < starttime)
            {
                endtime = DateTime.Now.AddMinutes(5);
            }

            TimeSpan ts = endtime - starttime;
            

            return Ok(new { success= true,ticket, Milliseconds = ts.TotalMilliseconds  });
                         
        }
    }
}

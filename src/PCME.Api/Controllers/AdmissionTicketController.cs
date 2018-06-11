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
    [Produces("application/json")]
    [Route("api/AdmissionTicket")]
    public class AdmissionTicketController:Controller
    {
        private readonly ApplicationDbContext context;
        public AdmissionTicketController(ApplicationDbContext context)
        {
            this.context = context;
            
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles="Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var admissionTicket = from admissionticket in context.AdmissionTickets
                                  join examsubject in context.ExamSubjects on admissionticket.ExamSubjectId equals examsubject.Id into left1
                                  from examsubject in left1.DefaultIfEmpty()
                                  join examinationroom in context.ExaminationRooms on admissionticket.ExaminationRoomId equals examinationroom.Id into left2
                                  from examinationroom in left2.DefaultIfEmpty()
                                  join trainingcenter in context.TrainingCenter on examinationroom.TrainingCenterId equals trainingcenter.Id into left3
                                  from trainingcenter in left3.DefaultIfEmpty()
                                  join signup in context.SignUp on admissionticket.SignUpId equals signup.Id into left4
                                  from signup in left4.DefaultIfEmpty()
                                  join examinationroomplan in context.ExaminationRoomPlans on admissionticket.ExaminationRoomPlanId equals examinationroomplan.Id into left5
                                  from examinationroomplan in left5.DefaultIfEmpty()
                                  where admissionticket.StudentId == studentId
                                  select new { examinationroomplan, admissionticket, examsubject, examinationroom,trainingcenter,signup };

            admissionTicket = admissionTicket
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = admissionTicket.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.admissionticket.Id},
                { "admissionticket.Num",c.admissionticket.Num},
                { "admissionticket.SignInTime",c.admissionticket.SignInTime},
                { "admissionticket.LoginTime",c.admissionticket.LoginTime},
                { "admissionticket.PostPaperTime",c.admissionticket.PostPaperTime},
                { "examsubject.Id",c.examsubject.Id},
                { "examsubject.Name",c.examsubject.Name},
                { "examinationroom.Id",c.examinationroom.Id},
                { "examinationroom.Name",c.examinationroom.Name},
                { "trainingcenter.Id",c.trainingcenter.Id},
                { "trainingcenter.Name",c.trainingcenter.Name},
                { "admissionticket.CreateTime",c.admissionticket.CreateTime},
                { "signup.CreateTime",c.signup.CreateTime},
                { "examinationroomplan.ExamStartTime",c.examinationroomplan.ExamStartTime},
                { "examinationroomplan.ExamEndTime",c.examinationroomplan.ExamEndTime}
            });
            var total = admissionTicket.Count();
            return Ok(new { total, data = result });
        }
    }
}

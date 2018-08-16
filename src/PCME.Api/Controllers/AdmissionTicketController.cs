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

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdmissionTicket")]
    public class AdmissionTicketController : Controller
    {
        private readonly ApplicationDbContext context;
        public AdmissionTicketController(ApplicationDbContext context)
        {
            this.context = context;

        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
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
                                  join examroomplanticket in context.ExamRoomPlanTicket on admissionticket.ExamRoomPlanTicketId equals examroomplanticket.Id into left6
                                  from examroomplanticket in left6.DefaultIfEmpty()
                                  where admissionticket.StudentId == studentId
                                  select new { examroomplanticket, examinationroomplan, admissionticket, examsubject, examinationroom, trainingcenter, signup };

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
                { "examinationroomplan.ExamEndTime",c.examinationroomplan.ExamEndTime},
                {"examroomplanticket.Id",c.examroomplanticket.Id},
                {"examroomplanticket.Num",c.examroomplanticket.Num}
            });
            var total = admissionTicket.Count();
            return Ok(new { total, data = result });
        }

        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Student")]
        public IActionResult Post([FromBody]AdmissionTicketCreateOrUpdateCommand command, string opertype) {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            if (opertype == "new")
            {
                command.SetId(0);
            }
            var plan = context.ExaminationRoomPlans.Where(c => c.Id == command.ExaminationRoomPlanId).FirstOrDefault();
            int plancount = context.AdmissionTickets.Where(c => c.ExaminationRoomPlanId == command.ExaminationRoomPlanId).Count();
            if (plancount >= plan.Galleryful)
            {
                return Ok(new { success = false, message = "超出场次设置的人数限制，生成失败" });
            }
            if (plancount >= 999)
            {
                return Ok(new { success = false, message = "超出系统允许的场次人数限制，生成失败" });
            }

            string num = command.ExamSubjectId + plan.Num + (plancount + 1).ToString().PadLeft(3, '0');
            AdmissionTicket ticket = new AdmissionTicket(num, studentId, command.ExaminationRoomId, command.SignUpId, command.ExamSubjectId
                , null, null, null, DateTime.Now, command.ExaminationRoomPlanId,command.ExamRoomPlanTicketId);

            var numisExists = context.AdmissionTickets.Where(c => c.Num == num).Any();
            if (numisExists)
            {
                return Ok(new { success = false, message = "存在相同的准考证号，生成失败" });
            }
            var examsubjectisExists = context.AdmissionTickets.Where(c => c.ExamSubjectId == ticket.ExamSubjectId && c.StudentId == ticket.StudentId).Any();
            if (examsubjectisExists)
            {
                return Ok(new { success = false, message = "存在相同科目的准考证号，生成失败" });
            }

            //更改报名标记 保存准考证
            var signup = context.SignUp.Where(c => c.Id == command.SignUpId).FirstOrDefault();
            signup.TicketChangeCreate();
            context.AdmissionTickets.Add(ticket);
            context.SignUp.Update(signup);

            //扣减选场权限次数1
            //var student = context.Students.Where(c => c.Id == studentId).FirstOrDefault();
            //if (student.TicketCtr <1)
            //{
            //    return Ok(new { success = false, message = "您的选场权限次数小于1，生成失败" });
            //}
            //student.SubtractTicketCtr();
            //context.Students.Update(student);
            var roomplanticket = context.ExamRoomPlanTicket.Where(c => c.Id == command.ExamRoomPlanTicketId).FirstOrDefault();
            if (roomplanticket == null) {
                return Ok(new { success = false, message = "没有找到考试券，生成失败" });
            } else if(roomplanticket.IsExpense){
                return Ok(new { success = false, message = "考试券已经被消费，生成失败" });
            }
            roomplanticket.DoExpense();
            context.ExamRoomPlanTicket.Update(roomplanticket);

            //添加准考证生成日志
            AdmissionTicketLogs logs = new AdmissionTicketLogs(num, studentId, command.ExaminationRoomId, command.SignUpId, command.ExamSubjectId
                , null, null, null, DateTime.Now, command.ExaminationRoomPlanId);
            context.AdmissionTicketLogs.Add(logs);

            context.SaveChanges();
            return Ok(new { success = true, message = "生成成功！" });
        }
    }
}

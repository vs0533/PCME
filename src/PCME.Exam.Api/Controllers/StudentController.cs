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
    [Route("api/Student")]
    public class StudentController:Controller
    {
        private readonly ApplicationDbContext context;
        public StudentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("readbyidcard")]
        [Authorize(Roles = "RoomAccount")]
        public IActionResult ReadByIdcard(string idcard)
        {
            var roomAccountId = int.Parse(User.FindFirstValue("AccountId"));
            var roomaccount = context.ExaminationRoomAccount.Find(roomAccountId);

            var studenttinfo = (from admissiontickets in context.AdmissionTickets
                               join students in context.Students on admissiontickets.StudentId equals students.Id
                               join workunits in context.WorkUnits on students.WorkUnitId equals workunits.Id
                               join examsubjects in context.ExamSubjects on admissiontickets.ExamSubjectId equals examsubjects.Id
                               join examinationroomplans in context.ExaminationRoomPlans on admissiontickets.ExaminationRoomPlanId equals examinationroomplans.Id
                               where students.IDCard == idcard && examinationroomplans.ExaminationRoomId == roomaccount.ExaminationRoomId
                               select new
                               {
                                   students.Id,
                                   studentname = students.Name,
                                   students.IDCard,
                                   workunitname = workunits.Name,
                                   examsubjectname = examsubjects.Name,
                                   examsubjectid = examsubjects.Id,
                                   admissiontickets.Num,
                                   imagesrc=students.Photo,
                                   roomplantid = admissiontickets.ExaminationRoomPlanId,
                                   roomplantnum = examinationroomplans.Num
                               }).FirstOrDefault();
            //var student = context.Students.Where(c => c.IDCard == idcard).FirstOrDefault();
            if (studenttinfo == null)
            {
                return Ok(new { success = false, message = "未找到人员信息,确认人员是否在正确场次签到" });
            }
            return Ok(new { success = true, data = studenttinfo });
        }
    }
}

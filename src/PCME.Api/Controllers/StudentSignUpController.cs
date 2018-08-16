using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PCME.Api.Controllers
{
    /// <summary>
    /// 准考证生成页面
    /// </summary>
    [Produces("application/json")]
    [Route("api/StudentSignUp")]
    public class StudentSignUpController:Controller
    {
        private readonly ApplicationDbContext context;
        public StudentSignUpController(ApplicationDbContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// 未生成准考证的报名
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [Route("read")]
        [HttpPost]
        [Authorize(Roles= "Student")]
        public IActionResult StoreRead(int start,int limit,string filter,string query,string navigates) {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));

            var signUp = from signup in context.SignUp
                         join trainingcenter in context.TrainingCenter on signup.TrainingCenterId equals trainingcenter.Id into left1
                         from trainingcenter in left1.DefaultIfEmpty()
                         join examsubject in context.ExamSubjects on signup.ExamSubjectId equals examsubject.Id// into left2
                         //from examsubject in left2.DefaultIfEmpty()
                         where signup.TicketIsCreate == false && signup.StudentId == studentId
                         select new { signup, trainingcenter, examsubject };

            signUp = signUp
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = signUp.ToList(); //signUp.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.signup.Id},
                { "examsubject.Id",c.examsubject.Id},
                { "examsubject.Name",c.examsubject.Name},
                { "trainingcenter.Id",c.trainingcenter.Id},
                { "trainingcenter.Name",c.trainingcenter.Name},
                { "signup.CreateTime",c.signup.CreateTime}
            });

            var total = signUp.Count();
            return Ok(new { total, data = result });
        }
        /// <summary>
        /// 选择场次
        /// </summary>
        /// <param name="traingcenterId">培训点ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("readplanbysignup")]
        [Authorize(Roles ="Student")]
        public IActionResult StoreRead(int trainingcenterId) {
            var plan = from examinationroomplan in context.ExaminationRoomPlans
                       join examinationroom in context.ExaminationRooms on examinationroomplan.ExaminationRoomId equals examinationroom.Id into left1
                       from examinationroom in left1.DefaultIfEmpty()
                       let selectedcount = (context.AdmissionTickets.Where(c=>c.ExaminationRoomPlanId == examinationroomplan.Id).Count())
                       where examinationroomplan.AuditStatusId == AuditStatus.Pass.Id
                       &&
                       (examinationroomplan.PlanStatusId == PlanStatus.Default.Id || examinationroomplan.PlanStatusId == PlanStatus.SelectStart.Id)
                       &&
                       (DateTime.Now >= examinationroomplan.SelectTime && DateTime.Now <= examinationroomplan.SelectFinishTime)
                       && examinationroomplan.TrainingCenterId == trainingcenterId
                       select new { examinationroomplan,examinationroom,selectedcount };
            var result = plan.Select(c => new Dictionary<string, object>
            {
                { "id",c.examinationroomplan.Id},
                { "examinationroomplans.Num",c.examinationroomplan.Num},
                { "examinationroomplans.Galleryful",c.examinationroomplan.Galleryful},
                { "examinationroomplans.SelectTime",c.examinationroomplan.SelectTime},
                { "examinationroomplans.SelectFinishTime",c.examinationroomplan.SelectFinishTime},
                { "examinationroomplans.SignInTime",c.examinationroomplan.SignInTime},
                { "examinationroomplans.ExamEndTime",c.examinationroomplan.ExamEndTime},
                { "examinationroomplans.ExamStartTime",c.examinationroomplan.ExamStartTime},
                { "examinationrooms.Id",c.examinationroom.Id},
                { "examinationrooms.Name",c.examinationroom.Name},
                { "examinationrooms.Description",c.examinationroom.Description},
                { "selectedcount",c.selectedcount},
            });
            return Ok(new { total = 0, data = result });
        }

        
    }
}

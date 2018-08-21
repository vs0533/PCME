using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using PCME.Exam.Api.Application.ParameBinder;
using PCME.Exam.Api.Extensions;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCME.Exam.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ExaminationRoomPlan")]
    public class ExaminationRoomPlanController : Controller
    {
        private readonly ApplicationDbContext context;
        public ExaminationRoomPlanController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "RoomAccount")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var roomAccountId = int.Parse(User.FindFirstValue("AccountId"));
            var roomaccount = context.ExaminationRoomAccount.Find(roomAccountId);
            var rooms = from examinationroom in context.ExaminationRooms
                        where examinationroom.Id == roomaccount.ExaminationRoomId
                        select examinationroom;

            var examinationRoomPlan = from examinationroomplans in context.ExaminationRoomPlans
                                      join examinationrooms in context.ExaminationRooms on examinationroomplans.ExaminationRoomId equals examinationrooms.Id into left2
                                      from examinationrooms in left2.DefaultIfEmpty()
                                      join auditstatus in context.AuditStatus on examinationroomplans.AuditStatusId equals auditstatus.Id into left3
                                      from auditstatus in left3.DefaultIfEmpty()
                                      join planstatus in context.PlanStatus on examinationroomplans.PlanStatusId equals planstatus.Id into left4
                                      from planstatus in left4.DefaultIfEmpty()
                                      where rooms.Select(c=>c.Id).Contains(examinationrooms.Id) && auditstatus.Id == AuditStatus.Pass.Id
                                      select new { examinationroomplans,examinationrooms,auditstatus,planstatus};


            examinationRoomPlan = examinationRoomPlan
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());
            var item = examinationRoomPlan.Skip(start).Take(limit);
            var result = item.ToList().Select(c=> new Dictionary<string, object>
            { 
                { "id",c.examinationroomplans.Id},
                { "examinationroomplans.Num",c.examinationroomplans.Num},
                { "examinationroomplans.Galleryful",c.examinationroomplans.Galleryful},
                { "examinationroomplans.SelectTime",c.examinationroomplans.SelectTime},
                { "examinationroomplans.SelectFinishTime",c.examinationroomplans.SelectFinishTime},
                { "examinationroomplans.SignInTime",c.examinationroomplans.SignInTime},
                { "examinationroomplans.ExamEndTime",c.examinationroomplans.ExamEndTime},
                { "examinationroomplans.ExamStartTime",c.examinationroomplans.ExamStartTime},
                { "examinationrooms.Id",c.examinationrooms.Id},
                { "examinationrooms.Name",c.examinationrooms.Name},
                { "auditstatus.Id",c.auditstatus.Id},
                { "auditstatus.Name",c.auditstatus.Name},
                { "planstatus.Id",c.planstatus.Id},
                { "planstatus.Name",c.planstatus.Name}
            });
            
            var total = examinationRoomPlan.Count();
            return Ok(new { total, data = result });
        }
        /// <summary>
        /// 场次开考
        /// </summary>
        /// <param name="roomplanid"></param>
        /// <param name="signintime"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("startexam")]
        [Authorize(Roles = "RoomAccount")]
        public IActionResult StartExaminationRoomPlan(int roomplanid,DateTime signintime) {
            var roomAccountId = int.Parse(User.FindFirstValue("AccountId"));
            var roomaccount = context.ExaminationRoomAccount.Find(roomAccountId);

            var roomplan = context.ExaminationRoomPlans.Where(c => c.Id == roomplanid && c.ExaminationRoomId == roomaccount.ExaminationRoomId).FirstOrDefault();
            if (roomplan == null)
            {
                return Ok(new { success = false, message = "登陆失效，请重新登陆" });
            }
            if (roomplan.AuditStatusId != AuditStatus.Pass.Id)
            {
                return Ok(new { success = false, message = "该场次没有审核通过，开考失败。" });
            }
            if (roomplan.PlanStatusId == PlanStatus.Close.Id || roomplan.PlanStatusId == PlanStatus.Over.Id)
            {
                return Ok(new { success = false, message = "场次状态为"+PlanStatus.From(roomplan.PlanStatusId).Name+"，开考失败。" });
            }

            if (roomplan.PlanStatusId == PlanStatus.SignInStart.Id)
            {
                return Ok(new { success = false, message = "已经开考！不要重复设置。" });
            }
            var startcount = context.ExaminationRoomPlans.Where(c => c.ExaminationRoomId == roomaccount.ExaminationRoomId && c.PlanStatusId == PlanStatus.SignInStart.Id).Count();
            if (startcount >= 2)
            {
                return Ok(new { success = false, message = "同时只能设置两个场次开考" });
            }



            roomplan.StartExam(signintime);
            context.ExaminationRoomPlans.Update(roomplan);
            context.SaveChanges();
            return Ok(new { success = true, message = "开考成功" });
        }
        [HttpPost]
        [Route("readinvigilate")]
        [Authorize(Roles ="RoomAccount")]
        public IActionResult ReadInvigilate(int roomplanid) {
            var studentitem = from admissiontickets in context.AdmissionTickets
                              join students in context.Students on admissiontickets.StudentId equals students.Id
                              where admissiontickets.ExaminationRoomPlanId == roomplanid
                              select new { admissiontickets, students };
            var result = studentitem.Select(c => new Dictionary<string, object> {
                {"admissiontickets.Id",c.admissiontickets.Id},
                {"admissiontickets.Num",c.admissiontickets.Num},
                {"admissiontickets.SignInTime",c.admissiontickets.SignInTime},
                {"admissiontickets.LoginTime",c.admissiontickets.LoginTime},
                {"students.IDCard",c.students.IDCard},
                {"students.Name",c.students.Name}
            });

            return Ok(new { total = result.Count(), data = result });
        }
    }
}

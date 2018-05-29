using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
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
        [Authorize(Roles = "TrainingCenter")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));

            var examinationRoomPlan = from examinationroomplans in context.ExaminationRoomPlans
                                      join examsubjects in context.ExamSubjects on examinationroomplans.ExamSubjectID equals examsubjects.Id into left1
                                      from examsubjects in left1.DefaultIfEmpty()
                                      join examinationrooms in context.ExaminationRooms on examinationroomplans.ExaminationRoomId equals examinationrooms.Id into left2
                                      from examinationrooms in left2.DefaultIfEmpty()
                                      join auditstatus in context.AuditStatus on examinationroomplans.AuditStatusId equals auditstatus.Id into left3
                                      from auditstatus in left3.DefaultIfEmpty()
                                      join planstatus in context.PlanStatus on examinationroomplans.PlanStatusId equals planstatus.Id into left4
                                      from planstatus in left4.DefaultIfEmpty()
                                      where examinationroomplans.TrainingCenterId == loginTrainingCenterId
                                      select new { examinationroomplans,examsubjects,examinationrooms,auditstatus,planstatus};
           
           examinationRoomPlan = examinationRoomPlan
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>()); ;
            var item = examinationRoomPlan.Skip(start).Take(limit);
            var result = item.ToList().Select(c=> new Dictionary<string, object>
            { 
                { "id",c.examinationroomplans.Id},
                { "examinationroomplans.Num",c.examinationroomplans.Num},
                { "examinationroomplans.SelectTime",c.examinationroomplans.SelectTime},
                { "examinationroomplans.SelectFinishTime",c.examinationroomplans.SelectFinishTime},
                { "examinationroomplans.SignInTime",c.examinationroomplans.SignInTime},
                { "examinationroomplans.ExamEndTime",c.examinationroomplans.ExamEndTime},
                { "examinationroomplans.ExamStartTime",c.examinationroomplans.ExamStartTime},
                { "examinationroomplans.ExamSubjectID",c.examinationroomplans.ExamSubjectID},
                { "examinationrooms.Id",c.examinationrooms.Id},
                { "examinationrooms.Name",c.examinationrooms.Name},
                { "examsubjects.Id",c.examsubjects.Id},
                { "examsubjects.Name",c.examsubjects.Name},
                { "auditstatus.Id",c.auditstatus.Id},
                { "auditstatus.Name",c.auditstatus.Name},
                { "planstatus.Id",c.planstatus.Id},
                { "planstatus.Name",c.planstatus.Name}
            });
            
            var total = examinationRoomPlan.Count();
            return Ok(new { total, data = result });
        }
    }
}

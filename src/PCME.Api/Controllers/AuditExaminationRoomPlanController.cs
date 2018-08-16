using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AuditExaminationRoomPlan")]
    public class AuditExaminationRoomPlanController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext context;
        public AuditExaminationRoomPlanController(ApplicationDbContext context,IMediator _mediator)
        {
            this.context = context;
            this._mediator = _mediator;
        }
        /// <summary>
        /// 主列表视图
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Admin")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var adminId = int.Parse(User.FindFirstValue("AccountId"));

            var examinationRoomPlan = from examinationroomplans in context.ExaminationRoomPlans
                                      join examinationrooms in context.ExaminationRooms on examinationroomplans.ExaminationRoomId equals examinationrooms.Id into left2
                                      from examinationrooms in left2.DefaultIfEmpty()
                                      join auditstatus in context.AuditStatus on examinationroomplans.AuditStatusId equals auditstatus.Id into left3
                                      from auditstatus in left3.DefaultIfEmpty()
                                      join planstatus in context.PlanStatus on examinationroomplans.PlanStatusId equals planstatus.Id into left4
                                      from planstatus in left4.DefaultIfEmpty()
                                      join trainingcenter in context.TrainingCenter on examinationroomplans.TrainingCenterId equals trainingcenter.Id into left5
                                      from trainingcenter in left5.DefaultIfEmpty()
                                      //where examinationroomplans.TrainingCenterId == loginTrainingCenterId
                                      select new { examinationroomplans,examinationrooms,auditstatus,planstatus,trainingcenter};
           
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
                { "planstatus.Name",c.planstatus.Name},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name}
            });
            
            var total = examinationRoomPlan.Count();
            return Ok(new { total, data = result });
        }
        /// <summary>
        /// 新增修改视图
        /// </summary>
        /// <param name="command"></param>
        /// <param name="opertype"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]ExaminationRoomPlanCreateOrUpdateCommand command, string opertype)
        {
            //var logintrainingcenterId = int.Parse(User.FindFirstValue("WorkUnitId"));

            //string roomnum = await (from c in context.ExaminationRooms
            //                        where c.Id == command.ExaminationRoomId
            //                        select c.Num).FirstOrDefaultAsync();
            //roomnum = roomnum.PadLeft(2, '0');
            //string num = roomnum + command.Num;

            //if (opertype == "new")
            //{
            //    command.SetId(0);
            //    Regex regex = new Regex(@"^0?[0-9]{6}$");
            //    bool valnum = regex.IsMatch(command.Num);
            //    if (!valnum)
            //    {
            //        ModelState.AddModelError("examinationroomplans.Num", "编号必须是6位数字字符分别由两位年+两位日序号+两位场次序号 如:180101代表18年第一天的第一场考试");
            //    }
            //}
            ModelState.Remove("opertype");
            command.AdminToSetInfo((int)command.AuditStatusId, (int)command.PlanStatusId);

            //var isaudit = await context.ExaminationRoomPlans.Where(c => c.Id == command.Id && c.AuditStatusId == AuditStatus.Pass.Id).AnyAsync();
            //if (isaudit)
            //{
            //    ModelState.AddModelError("num", "审核通过的场次不允许编辑");
            //    return BadRequest();
            //}

            //var numExists = await context.ExaminationRoomPlans.Where(c => c.Num == num && c.Id != command.Id).AnyAsync();
            //if (numExists)
            //{
            //    ModelState.AddModelError("examinationroomplans.Num", "相同编号的场次已经存在");
            //}
            if (command.SelectFinishTime <= command.SelectTime)
            {
                ModelState.AddModelError("examinationroomplans.SelectFinishTime", "结束选场时间不能早于等于开始选厂时间");
            }
            if (command.ExamEndTime <= command.ExamStartTime)
            {
                ModelState.AddModelError("examinationroomplans.ExamEndTime", "结束考试时间不能早于等于开始考试时间");
            }
            if (command.ExamStartTime <= command.SelectFinishTime)
            {
                ModelState.AddModelError("examinationroomplans.ExamStartTime", "开始考试时间不能早于等于结束选场时间");
            }

            if (ModelState.IsValid)
            {
                Dictionary<string, object> result = await _mediator.Send(command);
                return Ok(new { success = true, data = result });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

            var del = await context.ExaminationRoomPlans.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            if (del.AuditStatusId == AuditStatus.Pass.Id)
            {
                return Ok(new { message = "禁止删除已经审核通过的场次" });
            }
            #region 注意检测选场人员
            var ticket = await context.AdmissionTickets.Where(c => c.ExaminationRoomPlanId == del.Id).AnyAsync();
            if (ticket)
            {
                return Ok(new { message = "已经存在选择该场次的准考证，不允许删除" });
            }
            #endregion

            context.ExaminationRoomPlans.Remove(del);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

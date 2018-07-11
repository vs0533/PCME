using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Extensions;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Application.Commands;
using MediatR;
using System.Security.Claims;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ChangeStudentUnit")]
    public class ChangeStudentUnitController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator _mediator;
        public ChangeStudentUnitController(ApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this._mediator = mediator;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Read(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var search = from changestudentunit in context.ChangeStudentUnit
                         join student in context.Students on changestudentunit.StudentId equals student.Id
                         join oldunit in context.WorkUnits on changestudentunit.OldUnitId equals oldunit.Id
                         join newunit in context.WorkUnits on changestudentunit.NewUnitId equals newunit.Id
                         join auditstatus in context.AuditStatus on changestudentunit.AuditStatusId equals auditstatus.Id
                         where changestudentunit.NewUnitId == loginUnitId
                         orderby changestudentunit.CreateTime descending, changestudentunit.AuditStatusId ascending
                         select new { changestudentunit, student, oldunit, newunit, auditstatus };
            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var result = search.Select(c => new Dictionary<string, object>
            {
                {"id",c.changestudentunit.Id},
                {"student.IDCard",c.student.IDCard},
                {"student.Id",c.student.Id},
                {"student.Name",c.student.Name},
                {"oldunit.Id",c.oldunit.Id},
                {"oldunit.Name",c.oldunit.Name},
                {"newunit.Id",c.newunit.Id},
                {"newunit.Name",c.newunit.Name},
                {"changestudentunit.CreateTime",c.changestudentunit.CreateTime},
                {"changestudentunit.AuditStatusTime",c.changestudentunit.AuditStatusTime},
                {"auditstatus.Id",c.auditstatus.Id},
                {"auditstatus.Name",c.auditstatus.Name}
            });
            var total = await search.CountAsync();
            return Ok(new { total, data = result });
        }
        public class studentparment {
            [JsonProperty("student.IDCard")]
            public string IDCard { get; set; }
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Post([FromBody]studentparment studentparment , string opertype)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            ChangeStudentUnitCreateOrUpdateCommand command = new ChangeStudentUnitCreateOrUpdateCommand();
            if (opertype == "new")
            {
                command.SetId(0);
            }
            ModelState.Remove("opertype");
            var student = await context.Students.FirstOrDefaultAsync(c=>c.IDCard == studentparment.IDCard);
            if (student == null)
            {
                ModelState.AddModelError("student.IDCard", "未找到匹配该身份证的人员，调动申请失败！");
                return BadRequest();
            }
            if (student.WorkUnitId == loginUnitId)
            {
                ModelState.AddModelError("student.IDCard", "该人员已经在您的单位中了，不需要调动");
            }
            var isExists = await context.ChangeStudentUnit.Where(c => 
            c.StudentId == student.Id && c.AuditStatusId == AuditStatus.Wait.Id
            ).AnyAsync();
            if (isExists)
            {
                ModelState.AddModelError("student.IDCard", "相同人员的调动正在审批，请等待通过后再进行申请");
            }
            ModelState.Remove("opertype");
            if (ModelState.IsValid)
            {
                command.StudentId = student.Id;
                command.NewUnitId = loginUnitId;
                command.OldUnitId = student.WorkUnitId;
                command.AuditStatusId = AuditStatus.Wait.Id;

                Dictionary<string, object> result = await _mediator.Send(command);
                return Ok(new { success = true, data = result });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var id = data["id"].ToObject<int>();

            var del = await context.ChangeStudentUnit.FindAsync(id);
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            if (del.NewUnitId != loginUnitId)
            {
                return Ok(new { message = "非本单位申请不可以删除" });
            }
            if (del.AuditStatusId == AuditStatus.Pass.Id)
            {
                return Ok(new { message = "不能删除已经审核通过的记录" });
            }

            context.ChangeStudentUnit.Remove(del);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
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
    [Route("api/ExaminationRoomAccount")]
    public class ExaminationRoomAccountController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;
        public ExaminationRoomAccountController(ApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "TrainingCenter")]
        public IActionResult read(int start, int limit, string filter, string query, string navigates)
        {
            var logtrainingcenterid = int.Parse(User.FindFirstValue("AccountId"));
            var search = from examinationroomaccount in context.ExaminationRoomAccount
                        join trainingcenter in context.TrainingCenter on examinationroomaccount.TrainingCenterId equals trainingcenter.Id
                        join examinationroom in context.ExaminationRooms on examinationroomaccount.ExaminationRoomId equals examinationroom.Id
                        where examinationroomaccount.TrainingCenterId == logtrainingcenterid
                         orderby examinationroomaccount.CreateTime descending
                        select new { examinationroomaccount, trainingcenter, examinationroom };
            search = search
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());
            var item = search.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                {"examinationroomaccount.Id",c.examinationroomaccount.Id},
                {"examinationroomaccount.AccountName",c.examinationroomaccount.AccountName},
                {"examinationroomaccount.Password",c.examinationroomaccount.Password},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"examinationroom.Id",c.examinationroom.Id},
                {"examinationroom.Name",c.examinationroom.Name}
            });
            return Ok(result);
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> save([FromBody]ExaminationRoomAccountCreateOrUpdateCommand command, string opertype) {
            if (opertype == "new")
            {
                command.Id = 0;
            }
            ModelState.Remove("opertype");
            var logtrainingcenterid = int.Parse(User.FindFirstValue("AccountId"));
            var nameIsExists = context.ExaminationRoomAccount.FirstOrDefault(c => c.AccountName == command.AccountName && c.Id != command.Id);
            if (nameIsExists != null)
            {
                ModelState.AddModelError("examinationroomaccount.AccountName", "相同名称的账号已经存在");
            }
            command.TrainingCenterId = logtrainingcenterid;
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(command);
                return Ok(new { success=true,data = result});
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

            var del = await context.ExaminationRoomAccount.FindAsync(id);
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            context.ExaminationRoomAccount.Remove(del);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

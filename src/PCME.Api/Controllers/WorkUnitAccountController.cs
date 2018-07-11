﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Extensions;
using PCME.Api.Application.ParameBinder;
using System.Data.SqlClient;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.Commands;
using MediatR;
using Newtonsoft.Json.Linq;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnitAccount")]
    public class WorkUnitAccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator _mediator;
        public WorkUnitAccountController(ApplicationDbContext context, IMediator _mediator)
        {
            this.context = context;
            this._mediator = _mediator;
        }

        [HttpGet]
        [Route("navigatedata")]
        [Authorize(Roles = "Unit")]
        public IActionResult NavigateData(int? id, int? node)
        {
            node = node == 0 ? null : node;
            var _id = node ?? id ?? int.Parse(User.FindFirstValue("WorkUnitId"));

            var query = context.WorkUnits.Where(c => c.PID == _id).Include(s => s.Childs);
            if (node == null)
            {
                query = context.WorkUnits.Where(c => c.Id == _id).Include(s => s.Childs);
            }
            var item = query.Select(d => new
            {
                d.Id,
                text = d.Name,
                d.PID,
                leaf = !d.Childs.Any(),
                fieldvalue = d.Id
            });
            return Ok(item);
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Unit")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {

            var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
            var id = int.Parse(User.FindFirstValue("WorkUnitId"));

            var search = from workunitaccount in context.WorkUnitAccounts
                         join workunit in context.WorkUnits on workunitaccount.WorkUnitId equals workunit.Id into left1
                         from workunit in left1.DefaultIfEmpty()
                         join workunitaccounttype in context.WorkUnitAccountType on workunitaccount.WorkUnitAccountTypeId equals workunitaccounttype.Id into left2
                         from workunitaccounttype in left2.DefaultIfEmpty()
                         select new { workunitaccount, workunit, workunitaccounttype };

            var workunitid = navigate == null ? id : navigate.FieldValue;
            search = search.Where(c => c.workunitaccount.WorkUnitId == workunitid)
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);

            var result = item.ToList().Select(c => new Dictionary<string, object>
            {
                { "id",c.workunitaccount.Id},
                { "workunitaccount.AccountName",c.workunitaccount.AccountName},
                { "workunitaccount.PassWord",c.workunitaccount.PassWord},
                { "workunit.Id",c.workunit.Id},
                { "workunit.Name",c.workunit.Name},
                { "workunitaccount.HolderName",c.workunitaccount.HolderName},
                { "workunitaccounttype.Id",c.workunitaccounttype.Id},
                { "workunitaccounttype.Name",c.workunitaccounttype.Name}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }

        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Post([FromBody]WorkUnitAccountCreateOrUpdateCommand command, string navigates, string opertype)
        {
            var accountid = int.Parse(User.FindFirstValue("AccountId"));
            var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
            var role = User.Claims.Where(c => c.Type == "role");

            var account = await context.WorkUnitAccounts.Where(c => c.Id == accountid).FirstOrDefaultAsync();//.Include(s => s.WorkUnitAccountType).Include(s => s.WorkUnit);
            if (account == null)
            {
                ModelState.AddModelError("name", "未找到当前登陆");
            }
            if (!(role.Where(c => c.Value == "单位管理员").Any()))//如果不是单位管理员 则为当前账号的类型
            {
                command.SetWorkUnitAccountType(account.WorkUnitAccountTypeId);
            }
            command.SetUnitId(account.WorkUnitId);//设置账号所属单位

            if (opertype == "new")
            {
                command.SetId(0);
            }
            var nameExists = context.WorkUnitAccounts.Where(c => c.AccountName == command.AccountName && c.Id != command.Id).Any();
            if (nameExists)
            {
                ModelState.AddModelError("books.name", "相同名称的账号已经存在");
            }
            ModelState.Remove("opertype");
            if (ModelState.IsValid)
            {
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
            var id = data["id"].ToObject<int>();
            var workUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var delAccount = await context.WorkUnitAccounts.FindAsync(id);
            //var delUnit = await workUnitRepository.FindAsync(id);
            if (delAccount is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }

            if (delAccount.WorkUnitId != workUnitId)
            {
                return Ok(new { message = "非本单位账号不允许删除" });
            }

            if (delAccount.WorkUnitAccountTypeId != WorkUnitAccountType.Manager.Id)
            {
                return Ok(new { message = "只有具备【单位管理】权限的账号才可进行删除" });
            }

            //workUnitRepository.Delete(delUnit);
            context.WorkUnitAccounts.Remove(delAccount);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
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

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnitAccount")]
    public class WorkUnitAccountController:Controller
    {
        private readonly ApplicationDbContext context;
        public WorkUnitAccountController(ApplicationDbContext context)
        {
            this.context = context; 
        }

        [HttpGet]
        [Route("navigatedata")]
        [Authorize(Roles = "Unit")]
        public IActionResult NavigateData(int? id, int? node)
        {
            node = node == 0 ? null : node;
            var _id = node ?? id ?? int.Parse(User.FindFirstValue("WorkUnitId"));

            var query = context.WorkUnits.Where(c => c.PID == _id).Include(s=>s.Childs);
            if (node == null)
            {
                query = context.WorkUnits.Where(c => c.Id == _id).Include(s=>s.Childs);
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
        //[HttpPost]
        //[Route("read")]
        //[Authorize(Roles = "Unit")]
        //public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        //{

        //    var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
        //    var id = int.Parse(User.FindFirstValue("AccountId"));

        //    var query = from workunitaccount in context.WorkUnitAccounts
        //                join workunit in context.WorkUnits on workunitaccount.WorkUnitId equals workunit.Id into left1
        //                from workunit in left1.DefaultIfEmpty()
        //                select new { workunitaccount, workunit };
        //    var workunitid = navigate == null ? id : navigate.FieldValue;
        //    query = query.Where(c => c.workunitaccount.WorkUnitId == workunitid)
        //        .FilterAnd(filter.ToObject<Filter>())
        //        .FilterOr(query.ToObject<Filter>());

        //    var search = workUnitRepository.FromSql(sql, sqlparameId)
        //        .FilterAnd(filter.ToObject<Filter>())
        //        .FilterOr(query.ToObject<Filter>());
        //    var item = search.Skip(start).Take(limit);

        //    var result = item.ToList().Select(c => new Dictionary<string, object>
        //    {
        //        { "id",c.Id},
        //        { "name",c.Name},
        //        { "code",c.Code},
        //        { "Parent.Name",c.Parent?.Name},
        //        { "Parent.Id",c.PID},
        //        { "level",c.Level},
        //        { "linkman",c.LinkMan},
        //        { "linkphone",c.LinkPhone},
        //        { "WorkUnitNature.Name",WorkUnitNature.From(c.WorkUnitNatureId).Name},
        //        { "WorkUnitNature.Id",c.WorkUnitNatureId}
        //    });
        //    var total = search.Count();
        //    return Ok(new { total, data = result });
        //}
    }
}

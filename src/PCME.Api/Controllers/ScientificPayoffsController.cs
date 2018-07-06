using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// <summary>
    /// 科研/发明类
    /// </summary>
    [Produces("application/json")]
    [Route("api/ScientificPayoffs")]
    public class ScientificPayoffsController:Controller
    {
        public readonly ApplicationDbContext context;
        public ScientificPayoffsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var scientificPayoffsQuery = from scientificpayoffs in context.ScientificPayoffs
                                         join arealevel in context.AreaLevels on scientificpayoffs.AreaLevelId equals arealevel.Id into left1
                                         from arealevel in left1.DefaultIfEmpty()
                                         join awardsplevel in context.AwardSPLevels on scientificpayoffs.AwardSPLevelId equals awardsplevel.Id into left2
                                         from awardsplevel in left2.DefaultIfEmpty()
                                         join students in context.Students on scientificpayoffs.StudentId equals students.Id into left3
                                         from students in left3.DefaultIfEmpty()
                                         join auditstatus in context.AuditStatus on scientificpayoffs.AuditStateId equals auditstatus.Id into left4
                                         from auditstatus in left4.DefaultIfEmpty()
                                         where students.Id == studentId
                                         select new { scientificpayoffs, arealevel, awardsplevel,auditstatus };


            scientificPayoffsQuery = scientificPayoffsQuery
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = scientificPayoffsQuery.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.scientificpayoffs.Id},
                {"scientificpayoffs.Name",c.scientificpayoffs.Name},
                {"scientificpayoffs.ComplateDate",c.scientificpayoffs.ComplateDate},
                {"scientificpayoffs.Credit",c.scientificpayoffs.Credit},
                {"scientificpayoffs.JoinLevel",c.scientificpayoffs.JoinLevel},
                {"auditstatus.Id",c.auditstatus.Id},
                {"auditstatus.Name",c.auditstatus.Name},
                {"arealevel.Id",c.arealevel.Id},
                {"arealevel.Name",c.arealevel.Name},
                {"awardsplevel.Id",c.awardsplevel.Id},
                {"awardsplevel.Name",c.awardsplevel.Name}
            });
            var total = scientificPayoffsQuery.Count();
            return Ok(new { total, data = result });
        }
    }
}

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
    /// 论文/著作类
    /// </summary>
    [Produces("application/json")]
    [Route("api/Paper")]
    public class PaperController : Controller
    {
        private readonly ApplicationDbContext context;
        public PaperController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var paperQuery = from paper in context.Paper
                             join arealevel in context.AreaLevels on paper.AreaLevelId equals arealevel.Id into left1
                             from arealevel in left1.DefaultIfEmpty()
                             join awardpaperlevels in context.AwardPaperLevels on paper.AwardPaperLevelId equals awardpaperlevels.Id into left2
                             from awardpaperlevels in left2.DefaultIfEmpty()
                             join students in context.Students on paper.StudentId equals students.Id into left3
                             from students in left3.DefaultIfEmpty()
                             join auditstatus in context.AuditStatus on paper.AuditStatusId equals auditstatus.Id into left4
                             from auditstatus in left4.DefaultIfEmpty()
                             join periodicals in context.Periodicals on paper.PeriodicalId equals periodicals.Id into left5
                             from periodicals in left5.DefaultIfEmpty()
                             join publishtype in context.PublishTypes on paper.PublishTypeId equals publishtype.Id into left6
                             from publishtype in left6.DefaultIfEmpty()
                             where students.Id == studentId
                             select new { paper, publishtype, arealevel, awardpaperlevels, students, auditstatus, periodicals };
            paperQuery = paperQuery
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = paperQuery.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.paper.Id},
                {"paper.Name",c.paper.Name},
                {"paper.Credit",c.paper.Credit},
                {"paper.JoinCount",c.paper.JoinCount},
                {"paper.JoinLevel",c.paper.JoinLevel},
                {"publishtype.Id",c.publishtype.Id},
                {"publishtype.Name",c.publishtype.Name},
                {"paper.PublishDate",c.paper.PublishDate},
                {"periodicals.Id", c.periodicals == null ? 0 : c.periodicals.Id},
                {"periodicals.Name",c.periodicals == null ? "" : c.periodicals.Name},
                {"auditstatus.Name",c.auditstatus.Name},
                {"arealevel.Id",c.arealevel.Id},
                {"arealevel.Name",c.arealevel.Name},
                {"awardpaperlevels.Id",c.awardpaperlevels.Id},
                {"awardpaperlevels.Name",c.awardpaperlevels.Name}
            });
            var total = paperQuery.Count();
            return Ok(new { total, data = result });
        }
    }
}

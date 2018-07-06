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
    /// 学历/培训类（申报）
    /// </summary>
    [Produces("application/json")]
    [Route("api/CreditTrain")]
    public class CreditTrainController : Controller
    {
        public readonly ApplicationDbContext context;
        public CreditTrainController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var creditTrainQuery = from credittrain in context.CreditTrains
                                   join students in context.Students on credittrain.StudentId equals students.Id into left1
                                   from students in left1.DefaultIfEmpty()
                                   join auditstatus in context.AuditStatus on credittrain.AuditStatusId equals auditstatus.Id into left2
                                   from auditstatus in left2.DefaultIfEmpty()
                                   where students.Id == studentId
                                   select new { credittrain,auditstatus };


            creditTrainQuery = creditTrainQuery
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = creditTrainQuery.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                { "id",c.credittrain.Id},
                {"credittrain.AuditAccount",c.credittrain.AuditAccount},
                {"credittrain.Credit",c.credittrain.Credit},
                {"auditstatus.Id",c.auditstatus.Id},
                {"auditstatus.Name",c.auditstatus.Name},
                {"credittrain.Period",c.credittrain.Period},
                {"credittrain.Sponsor",c.credittrain.Sponsor},
                {"credittrain.TrainContent",c.credittrain.TrainContent},
                {"credittrain.TrainDate",c.credittrain.TrainDate},
                {"credittrain.TrainType",c.credittrain.TrainType}
            });
            var total = creditTrainQuery.Count();
            return Ok(new { total, data = result });
        }
    }
}

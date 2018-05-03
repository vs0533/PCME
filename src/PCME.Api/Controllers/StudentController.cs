using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.UnitAggregates;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Student")]
    public class StudentController : Controller
    {
        [HttpPost]
        [Route("read")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            return Ok();
            //var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
            //var id = User.FindFirstValue("id");
            #region
            //var sqlparameId = new SqlParameter("id", navigate == null ? id : navigate.FieldValue.ToString());
            ////var search = workUnitRepository.Query(c => c.Id != 0).Include(s=>s.Parent);
            //string sql = @"WITH temp  
            //                AS  
            //                (  
            //                SELECT * FROM Unit WHERE id = @id 
            //                UNION ALL  
            //                SELECT m.* FROM Unit  AS m  
            //                INNER JOIN temp AS child ON m.PID = child.Id  
            //                )  
            //                SELECT * FROM temp";
            //var search = workUnitRepository.FromSql(sql, sqlparameId)
            //    .FilterAnd(filter.ToObject<Filter>())
            //    .FilterOr(query.ToObject<Filter>());
            //var item = search.Skip(start).Take(limit);

            //var result = item.ToList().Select(c => new Dictionary<string, object>
            //{
            //    { "id",c.Id},
            //    { "name",c.Name},
            //    { "code",c.Code},
            //    { "Parent.Name",c.Parent?.Name},
            //    { "Parent.Id",c.PID},
            //    { "level",c.Level},
            //    { "linkman",c.LinkMan},
            //    { "linkphone",c.LinkPhone},
            //    { "password",c.PassWord},
            //    { "WorkUnitNature.Name",WorkUnitNature.From(c.WorkUnitNatureId).Name},
            //    { "WorkUnitNature.Id",c.WorkUnitNatureId}
            //});
            //var total = search.Count();
            //return Ok(new { total, data = result });
            #endregion
        }

    }
}

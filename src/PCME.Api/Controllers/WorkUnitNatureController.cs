using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.UnitAggregates;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnitNature")]
    public class WorkUnitNatureController : Controller
    {
        [HttpGet]
        [Route("read")]
        // GET: api/WorUnitNature
        public IActionResult StoreRead()
        {
            return Ok(WorkUnitNature.List().Select(c=>new { value =c.Id ,text=c.Name}));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data=WorkUnitNature.From(id)});
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnitAccountType")]
    public class WorkUnitAccountTypeController:Controller
    {
        [HttpGet]
        [Route("read")]
        // GET: api/OpenType
        public IActionResult StoreRead()
        {
            return Ok(WorkUnitAccountType.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data = WorkUnitAccountType.From(id) });
        }
    }
}

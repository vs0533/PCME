using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PlanStatus")]
    public class PlanStatusController:Controller
    {
        [HttpGet]
        [Route("read")]
        // GET: api/OpenType
        public IActionResult StoreRead()
        {
            return Ok(PlanStatus.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data = PlanStatus.From(id) });
        }
    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/AuditStatus")]
	public class AuditStatusController:Controller
    {
		[HttpGet]
        [Route("read")]
        // GET: api/AuditStatus
        public IActionResult StoreRead()
        {
            return Ok(AuditStatus.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
			return Ok(new { data = AuditStatus.From(id) });
        }
    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
    [Route("api/ExamType")]
	public class ExamTypeController:Controller
    {
		[HttpGet]
        [Route("read")]
        // GET: api/ExamType
        public IActionResult StoreRead()
        {
			return Ok(ExamType.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
			return Ok(new { data = ExamType.From(id) });
        }
    }
}

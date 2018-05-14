using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/ExamSubjectStatus")]
	public class ExamSubjectStatusController:Controller
    {
		[HttpGet]
        [Route("read")]
        // GET: api/ExamSubjectStatus
        public IActionResult StoreRead()
        {
            return Ok(ExamSubjectStatus.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
			return Ok(new { data = ExamSubjectStatus.From(id) });
        }
    }
}

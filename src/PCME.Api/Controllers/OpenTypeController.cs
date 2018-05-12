using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/OpenType")]
	public class OpenTypeController : Controller
	{
		[HttpGet]
        [Route("read")]
        // GET: api/OpenType
        public IActionResult StoreRead()
        {
			return Ok(OpenType.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data = OpenType.From(id) });
        }
	}
}

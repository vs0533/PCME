using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.StudentAggregates;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Sex")]
    public class SexController : Controller
    {
        [HttpGet]
        [Route("read")]
        // GET: api/Sex
        public IActionResult StoreRead()
        {
            return Ok(Sex.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data = Sex.From(id) });
        }
    }
}
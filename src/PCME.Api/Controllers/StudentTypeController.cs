using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.StudentAggregates;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/StudentType")]
    public class StudentTypeController : Controller
    {
        [HttpGet]
        [Route("read")]
        // GET: api/Sex
        public IActionResult StoreRead()
        {
            return Ok(StudentType.List().Select(c => new { value = c.Id, text = c.Name }));
        }
        [HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
            return Ok(new { data = StudentType.From(id) });
        }
    }
}

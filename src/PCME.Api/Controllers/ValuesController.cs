using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PCME.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            //return new string[] { "value1", "value2" };
            IEnumerable<Dictionary<string, dynamic>> d = new List<Dictionary<string, dynamic>>{
                new Dictionary<string, dynamic>{
                    { "id","1"},
                    { "text","水水"},
                    { "leaf",true},
                    { "route","/"}
                },
                new Dictionary<string, dynamic>{
                    { "id","2"},
                    { "text","得到"},
                    { "leaf",true},
                    { "route","/"}
                }
            };
            return Ok(d);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

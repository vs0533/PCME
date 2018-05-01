using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.Filters
{
    public class CheckPostParametersFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var json = new { message = "sadf", success = false, data = new string[] { "id", "name" } };
            //context.Result = new BadRequestObjectResult(json);
            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            if (!context.ModelState.IsValid)
            {
               
                StringBuilder sb = new StringBuilder("{");
                foreach (var key in context.ModelState.Keys)
                {
                    sb.Append("\"");
                    sb.Append(key);
                    sb.Append("\"");
                    sb.Append(":");
                    sb.Append("\"");
                    sb.Append(string.Join(";",context.ModelState[key].Errors.Select(d=>d.ErrorMessage)));
                    sb.Append("\"");
                    sb.Append(",");
                }
                sb.Append("}");

                var error = JsonConvert.DeserializeObject<dynamic>(sb.ToString());

                var json = new { message = "输入错误", success = false, data = error };
                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            
        }
    }
}

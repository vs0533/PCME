using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Exam.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdmissionTickets")]
    public class AdmissionTicketsController:Controller
    {
        private readonly ApplicationDbContext context;
        public AdmissionTicketsController(ApplicationDbContext context)
        {
            this.context = context; 
        }
        [HttpPost]
        [Route("signin")]
        [Authorize(Roles="RoomAccount")]
        public IActionResult SignIn(string num) {
            var admissionticket = context.AdmissionTickets.Where(c => c.Num == num).FirstOrDefault();
            if (admissionticket == null)
            {
                return Ok(new { success = false, message = "未找到对应的准考证号" });
            }
            admissionticket.SignIn();
            context.AdmissionTickets.Update(admissionticket);
            context.SaveChanges();
            return Ok(new { success = true, message = "签到成功" });
        }
    }
}

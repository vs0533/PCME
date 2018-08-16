using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/examroomplanticket")]

    public class ExamRoomPlanTicketController:Controller
    {
        private readonly ApplicationDbContext context;
        public ExamRoomPlanTicketController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ReadStore(int trainingcenterid) {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            var item = await context.ExamRoomPlanTicket.Where(c => 
            c.StudentId == studentid && 
            c.TrainingCenterId == trainingcenterid &&
            c.IsExpense == false
            ).ToListAsync();

            return Ok(new { total = item.Count(), data = item });
        }
    }
}

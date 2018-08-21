using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public IActionResult SignIn(string ticket) {

            var roomaccountid = int.Parse(User.FindFirstValue("AccountId"));
            var roomaccount = context.ExaminationRoomAccount.Find(roomaccountid);
            if (roomaccount == null)
            {
                return Ok(new { success = false, message = "超时，请重新登录" });
            }
            var admissionticket = context.AdmissionTickets.Where(c => c.Num == ticket).FirstOrDefault();

            var roomplan = context.ExaminationRoomPlans.Where(c => c.Id == admissionticket.ExaminationRoomPlanId).FirstOrDefault();
            if (DateTime.Now < roomplan.SignInTime.AddMinutes(-30) || DateTime.Now > roomplan.SignInTime.AddMinutes(20))
            {
                return Ok(new { success = false, message = string.Format("未在签到时间内不允许签到，签到时间为{0}-{1}",roomplan.SignInTime.AddMinutes(-30), roomplan.SignInTime.AddMinutes(20)) });
            }

            if (admissionticket == null)
            {
                return Ok(new { success = false, message = "未找到对应的准考证号" });
            }
            if (admissionticket.SignInTime != null)
            {
                return Ok(new { success = false, message = "该人员已经签到，不允许重复签到。" });
            }
            admissionticket.SignIn();
            context.AdmissionTickets.Update(admissionticket);
            context.SaveChanges();
            return Ok(new { success = true, message = "签到成功" });
        }
    }
}

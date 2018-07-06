using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
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
    [Route("api/CreditInfo")]
    public class CreditInfoController : Controller
    {
        public readonly ApplicationDbContext context;
        public CreditInfoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("PassInfo")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PassInfo()
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var professionalInfo = await context.ProfessionalInfos.Where(c => c.StudentId == studentId).Include(s => s.ProfessionalTitle).FirstOrDefaultAsync();

            var creditexam = await context.CreditExams.Where(c =>
                                        c.StudentId == studentId &&
                                        c.CreateTime >= professionalInfo.CalculateDate
                                    ).SumAsync(c => c.Credit);
            var credittrain = await context.CreditTrains.Where(c =>
                                        c.StudentId == studentId &&
                                        c.AuditStatusId == AuditStatus.Pass.Id &&
                                        c.TrainDate >= professionalInfo.CalculateDate
                                    ).SumAsync(c => c.Credit);
            var paper = await context.Paper.Where(c =>
                                        c.StudentId == studentId &&
                                        c.AuditStatusId == AuditStatus.Pass.Id &&
                                        c.PublishDate >= professionalInfo.CalculateDate
                                    ).SumAsync(c => c.Credit);
            var sp = await context.ScientificPayoffs.Where(c =>
                                        c.StudentId == studentId &&
                                        c.AuditStateId == AuditStatus.Pass.Id &&
                                        c.ComplateDate >= professionalInfo.CalculateDate
                                    ).SumAsync(c => c.Credit);

            DateTime now = DateTime.Now;
            DateTime calculateDate = professionalInfo.CalculateDate;
            var jfzq = ((now.Year - calculateDate.Year) * 12 + now.Month - calculateDate.Month - 1) / 6 / 2.0;
            float jfzqFormat = float.Parse(jfzq.ToString("0.0"));

            float creditall = (float)((creditexam) + (credittrain) + (paper) + (sp));
            float creditPass = 20 * jfzqFormat;

            var result = new Dictionary<string, object>
            {
                {"ProfessionalTitle",professionalInfo.ProfessionalTitle.Name},
                {"GetDate",professionalInfo.GetDate.ToString("yyyy-MM-dd")},
                {"CalculateDate",professionalInfo.CalculateDate.ToString("yyyy-MM-dd")},
                {"EndDate",DateTime.Now.ToString("yyyy-MM-dd") },
                {"creditall",creditall},
                {"creditpass",creditPass},
                {"jfzq",jfzqFormat},
                {"cf",(creditPass - creditall) < 0 ? 0 : (creditPass - creditall)}
            };
            return Ok(new { success = true, data = result });
        }
    }
}

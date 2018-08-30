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
            var student = await context.Students.Include(s=>s.WorkUnit).Where(c=>c.Id == studentId).FirstOrDefaultAsync();
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

            var creditexam14_18 = (from creditexam_ in context.CreditExams
                                  join examsubjects in context.ExamSubjects on creditexam_.SubjectId equals examsubjects.Id
                                  where creditexam_.StudentId == student.Id && creditexam_.CreateTime >= new DateTime(2014, 12, 31) && creditexam_.CreateTime <= new DateTime(2019, 1, 1)
                                  select new { creditexam_.AdmissionTicketNum, examsubjects.Name, creditexam_.Credit });
            var creditexam14_18_result = await creditexam14_18.Take(8).ToListAsync();
            var count = creditexam14_18.Count();
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
                {"cf",(creditPass - creditall) < 0 ? 0 : (creditPass - creditall)},
                {"studentid", student.Id},
                {"studentname",student.Name},
                {"idcard",student.IDCard },
                {"studentsex",student.Sex == null ? "未定义": student.Sex.Name},
                {"workunitname",student.WorkUnit.Name},
                {"yxqdate",DateTime.Now.AddDays(180).ToString("yyyy-MM-dd")},
                {"creditexam",creditexam14_18_result.Select(c=>new Dictionary<string,object>{
                    {"admissionticketnum",c.AdmissionTicketNum },
                    {"examsubjectname",c.Name },
                    {"credit",c.Credit}
                }) },
                {"count",count}
            };
            return Ok(new { success = true, data = result });
        }
        [HttpPost]
        [Route("PassInfoforAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PassInfoforAdmin(int id)
        {
            var studentId = id; //int.Parse(User.FindFirstValue("AccountId"));
            var student = await context.Students.Include(s => s.WorkUnit).Where(c => c.Id == studentId).FirstOrDefaultAsync();
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

            var creditexam14_18 = (from creditexam_ in context.CreditExams
                                   join examsubjects in context.ExamSubjects on creditexam_.SubjectId equals examsubjects.Id
                                   where creditexam_.StudentId == student.Id && creditexam_.CreateTime >= new DateTime(2013, 12, 31) && creditexam_.CreateTime <= new DateTime(2019, 1, 1)
                                   select new { creditexam_.AdmissionTicketNum, examsubjects.Name, creditexam_.Credit });
            var creditexam14_18_result = await creditexam14_18.Take(8).ToListAsync();
            var count = creditexam14_18.Count();
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
                {"cf",(creditPass - creditall) < 0 ? 0 : (creditPass - creditall)},
                {"studentid", student.Id},
                {"studentname",student.Name},
                {"idcard",student.IDCard },
                {"studentsex",student.Sex == null ? "未定义": student.Sex.Name},
                {"workunitname",student.WorkUnit.Name},
                {"yxqdate",DateTime.Now.AddDays(180).ToString("yyyy-MM-dd")},
                {"creditexam",creditexam14_18_result.Select(c=>new Dictionary<string,object>{
                    {"admissionticketnum",c.AdmissionTicketNum },
                    {"examsubjectname",c.Name },
                    {"credit",c.Credit}
                }) },
                {"count",count}
            };
            return Ok(new { success = true, data = result });
        }
        [HttpPost]
        [Route("read1")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Read1()
        {
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            return Ok();
        }
    }
}

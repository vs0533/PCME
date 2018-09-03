using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Extensions;
using PCME.Api.Application.ParameBinder;
using Newtonsoft.Json.Linq;
using PCME.Domain.AggregatesModel.ExaminationRoomPlantTicketAggregates;
using Newtonsoft.Json;

namespace PCME.Api.Controllers
{
    public class StudentDTO
    {
        [JsonProperty("students.IDCard")]
        public string IDCard { get; set; }
    }
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
        [Route("readbytrainingcenterid")]
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
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "TrainingCenter")]
        public IActionResult ReadStore(int start, int limit, string filter, string query, string navigates)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var search = from examroomplanticket in context.ExamRoomPlanTicket
                         join trainingcenter in context.TrainingCenter on examroomplanticket.TrainingCenterId equals trainingcenter.Id
                         join students in context.Students on examroomplanticket.StudentId equals students.Id
                         where examroomplanticket.TrainingCenterId == loginTrainingCenterId
                         select new { examroomplanticket, trainingcenter, students };

            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);
            var total = search.Count();
            var result = search.Select(c => new Dictionary<string, object>
            {
                {"id",c.examroomplanticket.Id},
                {"examroomplanticket.Num",c.examroomplanticket.Num},
                {"examroomplanticket.CreateTime",c.examroomplanticket.CreateTime},
                {"examroomplanticket.IsExpense",c.examroomplanticket.IsExpense},
                {"examroomplanticket.ExpenseTime",c.examroomplanticket.ExpenseTime},
                {"examroomplanticket.ReMark",c.examroomplanticket.ReMark},
                {"students.Id",c.students.Id},
                {"students.Name",c.students.Name},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"students.IDCard",c.students.IDCard}
            });
            return Ok(new { total, data = result });
        }
        
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> Post([FromBody]StudentDTO dto, string opertype)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var student = await context.Students.Where(c => c.IDCard == dto.IDCard).FirstOrDefaultAsync();
            if (student == null)
            {
                ModelState.AddModelError("examroomplanticket.IDCard", "身份证号不存在");
            }
            ModelState.Remove("opertype");
            if (ModelState.IsValid)
            {
                ExamRoomPlanTicket ticket = new ExamRoomPlanTicket(Guid.NewGuid().ToString().Replace("-", ""), student.Id, loginTrainingCenterId);
                context.ExamRoomPlanTicket.Add(ticket);
                await context.SaveChangesAsync();

                var search = from examroomplanticket in context.ExamRoomPlanTicket
                             join trainingcenter in context.TrainingCenter on examroomplanticket.TrainingCenterId equals trainingcenter.Id
                             join students in context.Students on examroomplanticket.StudentId equals students.Id
                             where examroomplanticket.Id == ticket.Id
                             select new { examroomplanticket, trainingcenter, students };

                var result = search.Select(c => new Dictionary<string, object>
            {
                {"id",c.examroomplanticket.Id},
                {"examroomplanticket.Num",c.examroomplanticket.Num},
                {"examroomplanticket.CreateTime",c.examroomplanticket.CreateTime},
                {"examroomplanticket.IsExpense",c.examroomplanticket.IsExpense},
                {"examroomplanticket.ExpenseTime",c.examroomplanticket.ExpenseTime},
                {"examroomplanticket.ReMark",c.examroomplanticket.ReMark},
                {"students.Id",c.students.Id},
                {"students.Name",c.students.Name},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"students.IDCard",c.students.IDCard}
            });
                return Ok(new { success = true, data = result });
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

            var del = await context.ExamRoomPlanTicket.FindAsync(id);
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            if (del.IsExpense)
            {
                return Ok(new { message = "已经消费的考试券不允许删除" });
            }

            context.ExamRoomPlanTicket.Remove(del);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Api.Infrastructure;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/StudentMe")]
    public class StudentMeController : Controller
    {
        private IHostingEnvironment hostingEnv;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<WorkUnit> workUnitRepository;
        private readonly TestDBContext testContext;
        private readonly ApplicationDbContext context;
        public StudentMeController(ApplicationDbContext context,TestDBContext testContext,IHostingEnvironment hostingEnv,IUnitOfWork<ApplicationDbContext> unitOfWork, IMediator _mediator)
        {
            this.unitOfWork = unitOfWork;
            studentRepository = unitOfWork.GetRepository<Student>();
            workUnitRepository = unitOfWork.GetRepository<WorkUnit>();
            this._mediator = _mediator;
            this.hostingEnv = hostingEnv;
            this.testContext = testContext;
            this.context = context;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginStudentId = int.Parse(User.FindFirstValue("AccountId"));
            var search = studentRepository.Query(f => f.Id == loginStudentId,
                include: s => s
                 .Include(c => c.StudentType)
                 .Include(c => c.Sex)
                 .Include(c => c.StudentStatus))
                 .Include(c => c.WorkUnit)
                 .FilterAnd(filter.ToObject<Filter>())
                 .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);


            var result = item.Select(c => new Dictionary<string, object> {
                { "id",c.Id},
                { "name",c.Name},
                { "idcard",c.IDCard},
                { "officename",c.OfficeName},
                { "password",c.Password},
                { "Sex.Id",c.SexId},
                { "Sex.Name",c.Sex.Name},
                { "StudentType.Id",c.StudentTypeId},
                { "StudentType.Name",c.StudentType.Name},
                { "StudentStatus.Id",c.StudentStatusId},
                { "StudentStatus.Name",c.StudentStatus.Name},
                { "graduationschool",c.GraduationSchool},
                { "specialty",c.Specialty},
                { "workdate",c.WorkDate},
                { "WorkUnit.Id",c.WorkUnitId},
                { "WorkUnit.Name",c.WorkUnit.Name},
                { "address",c.Address},
                { "email",c.Email},
                { "birthday",c.BirthDay},
                { "favicon",ImageHelper.ImgToBase64String(Path.Combine(hostingEnv.WebRootPath,"Files",(c.Photo ?? "")))},
                {"joinedu",c.JoinEdu}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }


        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Post([FromBody]StudentCreateOrUpdateCommand command, string opertype)
        {
            var loginStudentId = int.Parse(User.FindFirstValue("AccountId"));
            var loginWorkUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginWorkUnit = await workUnitRepository.FindAsync(loginWorkUnitId);
            command.SetWorkUnitId(loginWorkUnitId);

            if (opertype == "new")
            {
                command.SetId(0);
            }
            command.SetId(loginStudentId);

            var idCardExisted = studentRepository.GetFirstOrDefault(predicate: c => c.IDCard == command.IDCard && c.Id != command.Id);
            if (idCardExisted != null)
            {
                ModelState.AddModelError("idcard", "身份证号已经存在");
            }
            ModelState.Remove("opertype");
            if (ModelState.IsValid)
            {
                
                Student result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
                    { "id",result.Id},
                    { "name",result.Name},
                    { "idcard",result.IDCard},
                    { "officename",result.OfficeName},
                    { "password",result.Password},
                    { "Sex.Id",result.SexId},
                    { "Sex.Name",Sex.From(result.SexId).Name},
                    { "StudentType.Id", result.StudentTypeId},
                    { "StudentType.Name",StudentType.From(result.StudentTypeId).Name},
                    { "StudentStatus.Id",result.StudentStatusId},
                    { "StudentStatus.Name",StudentStatus.From(result.StudentStatusId).Name},
                    { "graduationschool",result.GraduationSchool},
                    { "specialty",result.Specialty},
                    { "workdate",result.WorkDate},
                    { "WorkUnit.Id",result.WorkUnitId},
                    { "WorkUnit.Name",result.WorkUnit.Name},
                    { "address",result.Address},
                    { "email",result.Email},
                    { "birthday",result.BirthDay},
                    { "favicon",ImageHelper.ImgToBase64String(Path.Combine(hostingEnv.WebRootPath,"Files",result.Photo))},
                    {"joinedu",result.JoinEdu}
                    
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();

        }


        [HttpPost]
        [Route("readexamresult")]
        [Authorize(Roles = "Student")]
        public IActionResult ReadExamResult(int start, int limit, string filter, string query, string navigates)
        {
            var loginStudentId = int.Parse(User.FindFirstValue("AccountId"));
            //取得考试成绩
            var examresult = (from examresult_ in testContext.ExamResult
                              where examresult_.StudentId == loginStudentId
                              select examresult_).ToList();
            //取得作业成绩
            //var homeworkresult1 = testContext.HomeWorkResult.Where(c => c.StudentId == loginStudentId).ToList();
            //取得带科目ID的作业成绩
            //var homeworkresult2 = (from homeworkresult_ in homeworkresult1
            //                       join examsubject in context.ExamSubjects on homeworkresult_.CategoryCode equals examsubject.Code
            //                       group new { examsubject.Id, homeworkresult_.Score, homeworkresult_.StudentId } by new { examsubject.Id, homeworkresult_.StudentId } into g
            //                       select new { g.Key.Id, g.Key.StudentId, homeworkscore= g.Sum(c => c.Score) });
                                   //select new { g.k homeworkresult_.Id, homeworkresult_.CategoryCode, homeworkresult_.Score, examsubjectid = examsubject.Id }).ToList();
            //联合成绩
            var search = from examresult1 in examresult
                         join examsubjects in context.ExamSubjects on examresult1.ExamSubjectId equals examsubjects.Id
                         //join homeworkresult in homeworkresult2 on examresult1.ExamSubjectId equals homeworkresult.Id into left2
                         //from homeworkresult in left2.DefaultIfEmpty()
                         orderby examresult1.CreateTime descending
                         select new { examresult1.Id, examresultscore=examresult1.Score, examresult1.TicketNum, examresult1.CreateTime, examsubjects.Name};
            //search = search
            //     .FilterAnd(filter.ToObject<Filter>())
            //     .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);


            var result = item.Select(c => new Dictionary<string, object> {
                { "id",c.Id},
                { "score",c.examresultscore},
                { "examsubjectname",c.Name},
                { "ticketnum",c.TicketNum},
                { "createtime",c.CreateTime}//,
                //{"homeworkscore",Math.Round(c.homeworkscore,2)}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }

    }
}

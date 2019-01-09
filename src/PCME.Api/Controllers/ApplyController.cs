using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PCME.Domain.SeedWork;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Apply")]
    public class ApplyController : Controller
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly IMediator _mediator;

        private readonly IRepository<WorkUnitAccount> workUnitAccountRepository;
        private readonly IRepository<Student> studentRepository;

        public ApplyController(ApplicationDbContext context, IMediator _mediator,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            this._mediator = _mediator;
            this.workUnitAccountRepository = this.unitOfWork.GetRepository<WorkUnitAccount>();
            this.studentRepository = this.unitOfWork.GetRepository<Student>();
        }

        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Read(int start, int limit, string filter, string query, string navigates)
        {

            var search = context.ApplyForSetting.AsQueryable();
            var examsubjectItem = await context.ExamSubjects.ToListAsync();

            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var result = search.ToList().Select(c => new Dictionary<string, object>
            {
                {"id",c.Id},
                {"title",c.Title},
                {"examSubjectNameItem",string.Join(',',examsubjectItem.Where(w=>c.SubjectIds.Contains(w.Id)).Select(s=>s.Name))},
                {"starttime",c.StartTime},
                {"endtime",c.EndTime}
            });
            var total = await search.CountAsync();
            return Ok(new { total, data = result });
        }

        [HttpPost]
        [Route("readselect")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadSelect(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginAccountId = int.Parse(User.FindFirstValue("AccountId"));

            var curWorkUnit = workUnitAccountRepository.Find(loginAccountId);
            StudentType type = StudentType.Professional;

            if (curWorkUnit.WorkUnitAccountTypeId == WorkUnitAccountType.CS.Id)
            {
                type = StudentType.CivilServant;
            }
            else
            {
                type = StudentType.Professional;
            }


            var search = studentRepository.Query(f =>
                (f.WorkUnitId == loginUnitId && f.StudentStatusId == StudentStatus.Normal.Id && (f.StudentTypeId == type.Id || f.JoinEdu == true)),
                include: s => s
                 .Include(c => c.StudentType)
                 .Include(c => c.Sex)
                 .Include(c => c.StudentStatus)
                 .Include(c => c.WorkUnit));

            var filterObject = filter.ToObject<Filter>().FirstOrDefault();
            if (filterObject != null)
            {
                string value = filterObject.Value;
                search = search.Where(c => c.Name.Contains(value) || c.IDCard.Contains(value));
            }
            //.FilterOr(query.ToObject<Filter>());

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
                { "birthday",c.BirthDay}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }

        [HttpPost]
        [Route("readSelectedItem")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadSelectedItem(int? applyTableId)
        {
            //var search = signUpCollectionRepository.Query(c => c.Id != 0, include: c => c.Include(s => s.ExamSubject).Include(s => s.Student));
            //search = search.Where(c => c.SignUpForUnitId == signupforunitid);

            var search = context.ApplyTable.Include(i => i.StudentItems).FirstOrDefault(c => c.Id == applyTableId);
            if (search == null)
            {
                return Ok(new { total=0, data = "" });
            }
            //var item = search.Skip(start).Take(limit);
            var result = search.StudentItems.ToList().Select(c => new Dictionary<string, object>
                {
                    { "id",c.Id},
                    { "studentidcard",c.Student?.IDCard},
                    { "studentname",c.Student?.Name},
                    { "studentid",c.Student?.Id}
                });
            var total = search.StudentItems.Count();
            return Ok(new { total, data = result });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Student")]
    public class StudentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<WorkUnit> workUnitRepository;
        private readonly IRepository<WorkUnitAccount> workUnitAccountRepository;
        public StudentController(IUnitOfWork<ApplicationDbContext> unitOfWork, IMediator _mediator)
        {
            this.unitOfWork = unitOfWork;
            studentRepository = unitOfWork.GetRepository<Student>();
            workUnitRepository = unitOfWork.GetRepository<WorkUnit>();
            workUnitAccountRepository = unitOfWork.GetRepository<WorkUnitAccount>();
            this._mediator = _mediator;
            
        }
        [HttpPost]
        [Route("changepwd")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> ChangePwd(string oldpwd,string newpwd)
        {
            //var req = JsonConvert.DeserializeObject(str);
            var studentId = int.Parse(User.FindFirstValue("AccountId"));
            var student = await studentRepository.Query(predicate: c => c.Id == studentId).FirstOrDefaultAsync();
            if (student == null)
            {
                return BadRequest(new { success = false, message = "请登录" });
            }
            if (student.Password != oldpwd)
            {
                return BadRequest(new { success = false, message = "旧密码不正确" });
            }
            student.ChangePwd(newpwd);
            await unitOfWork.SaveChangesAsync();
            return Ok(new { success=true,message="修改成功"});
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Unit")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginAccountId = int.Parse(User.FindFirstValue("AccountId"));

            var account = workUnitAccountRepository.Find(loginAccountId);
            StudentType studentType = StudentType.Professional;
            switch (account.WorkUnitAccountTypeId)
            {
                case 4:
                    studentType = StudentType.CivilServant;
                    break;
                default:
                    studentType = StudentType.Professional;
                    break;
            }


            var search = studentRepository.Query(f => f.WorkUnitId == loginUnitId,
                include: s => s
                 .Include(c => c.StudentType)
                 .Include(c => c.Sex)
                 .Include(c => c.StudentStatus))
                 .Include(c => c.WorkUnit)
                 .FilterAnd(filter.ToObject<Filter>())
                 .FilterOr(query.ToObject<Filter>()).Where(c=>c.StudentTypeId == studentType.Id);

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
                {"joinedu",c.JoinEdu }
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }


        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Post([FromBody]StudentCreateOrUpdateCommand command, string opertype)
        {
            var loginWorkUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginWorkUnit = await workUnitRepository.FindAsync(loginWorkUnitId);
            command.SetWorkUnitId(loginWorkUnitId);

            if (opertype == "new")
            {
                command.SetId(0);
            }

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
                    {"joinedu",result.JoinEdu}
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();

        }
        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();
            //var loginid = int.Parse(User.FindFirstValue("id"));
            var delStudent = await studentRepository.FindAsync(id);
            if (delStudent is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            if (StudentStatus.From(delStudent.StudentStatusId) != StudentStatus.Normal)
            {
                return Ok(new { message = string.Format("该人员已经{0}",StudentStatus.From(delStudent.StudentStatusId).Name) });
            }
            delStudent.ChangeStudentStatus(StudentStatus.BeNotIn.Id);
            studentRepository.Update(delStudent);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

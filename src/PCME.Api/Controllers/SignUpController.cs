using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/SignUp")]
    public class SignUpController : Controller
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        public SignUpController(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Route("readstudentlist")]
        [Authorize]
        public IActionResult ReadStudentList(int start, int limit, string filter, string query, string navigates)
        {
            var studentRepository = unitOfWork.GetRepository<Student>();
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginAccountId = int.Parse(User.FindFirstValue("AccountId"));

            var workUnitAccountRepository = unitOfWork.GetRepository<WorkUnitAccount>();
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
                f.WorkUnitId == loginUnitId && f.StudentStatusId == StudentStatus.Normal.Id && f.StudentTypeId == type.Id,
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
        [Route("readtrainingcenterlist")]
        [Authorize]
        public IActionResult ReadTrainingCenterList(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginAccountId = int.Parse(User.FindFirstValue("AccountId"));

            var workUnitAccountRepository = unitOfWork.GetRepository<WorkUnitAccount>();
            var curWorkUnit = workUnitAccountRepository.Find(loginAccountId);
            OpenType type = OpenType.Professional;

            if (curWorkUnit.WorkUnitAccountTypeId == WorkUnitAccountType.CS.Id)
            {
                type = OpenType.CivilServant;
            }
            else
            {
                type = OpenType.Professional;
            }

            var examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
            var search = examSubjectOpenInfoRepository.Query(c =>
                c.AuditStatusId == AuditStatus.Pass.Id && c.TrainingCenter.OpenTypeId == type.Id
                &&
                (DateTime.Now > c.SignUpTime && DateTime.Now < c.SignUpFinishTime)
            )
                                                      .Include(s => s.AuditStatus)
                                                      .Include(s => s.ExamSubject)
                                                      .Include(s => s.TrainingCenter)
                                                        .ThenInclude(s=>s.OpenType)
                                              .FilterAnd(filter.ToObject<Filter>())
                                              .FilterOr(query.ToObject<Filter>());


            var item = search.Skip(start).Take(limit);
            var result = item.ToList().Select(c => new Dictionary<string, object>
                {
                    { "id",c.TrainingCenterId},
                    { "name",c.TrainingCenter.Name},
                    { "address",c.TrainingCenter?.Address},
                    { "OpenType.Id",c.TrainingCenter.OpenTypeId},
                    { "OpenType.Name",c.TrainingCenter.OpenType.Name}
                });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
        [HttpPost]
        [Route("readexamsubjectopeninfo")]
        [Authorize]
        public IActionResult ReadExamSubjectOpenInfo(int trainingId, int start, int limit, string filter, string query, string navigates)
        {
            var examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
            var search = examSubjectOpenInfoRepository.Query(c =>
                c.TrainingCenterId == trainingId && c.AuditStatusId == AuditStatus.Pass.Id
                &&
                (DateTime.Now > c.SignUpTime && DateTime.Now < c.SignUpFinishTime)
            )
                                                      .Include(s => s.AuditStatus)
                                                      .Include(s => s.ExamSubject)
                                                      .Include(s => s.TrainingCenter)
                                              .FilterAnd(filter.ToObject<Filter>())
                                              .FilterOr(query.ToObject<Filter>());


            var item = search.Skip(start).Take(limit);
            var result = item.ToList().Select(c => new Dictionary<string, object>
                {
                    {"id",c.Id},
                    {"AuditStatus.Id",c.AuditStatusId},
                    {"AuditStatus.Name",c.AuditStatus.Name},
                    {"ExamSubject.Id",c.ExamSubjectId},
                    {"ExamSubject.Name",c.ExamSubject.Name},
                    {"TrainingCenter.Name",c.TrainingCenter.Name},
                    {"TrainingCenter.Id",c.TrainingCenterId},
                    {"signuptime",c.SignUpTime},
                    {"signupfinishtime",c.SignUpFinishTime},
                    {"signupfinishoffset",c.SignUpFinishOffset},
                    {"displayexamtime",c.DisplayExamTime}
                });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
    }
}

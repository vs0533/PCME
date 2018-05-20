using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Api.Infrastructure.Excel;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.SignUpAggregates;
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
        private readonly ApplicationDbContext context;
        public SignUpController(IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext context)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
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
                                                        .ThenInclude(s => s.OpenType)
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
        [Route("readsignupcollection")]
        public IActionResult ReadSignUpCollection(int? signupforunitid)
        {
            var signUpCollectionRepository = unitOfWork.GetRepository<SignUpCollection>();
            var search = signUpCollectionRepository.Query(c => c.Id != 0, include: c => c.Include(s => s.ExamSubject).Include(s => s.Student));
            search = search.Where(c => c.SignUpForUnitId == signupforunitid);
            //var item = search.Skip(start).Take(limit);
            var result = search.ToList().Select(c => new Dictionary<string, object>
                {
                    { "id",c.Id},
                    { "studentidcard",c.Student?.IDCard},
                    { "studentname",c.Student?.Name},
                    { "studentid",c.Student?.Id},
                    { "examsubjectname",c.ExamSubject?.Name},
                    { "examsubjectid",c.ExamSubject?.Id},
                    { "signupforunitid",c.SignUpForUnitId},
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
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize]
        public IActionResult SaveOrUpdate([FromQuery]int? signupforunitid, [FromQuery]int trainingcenterid, [FromBody]Object data)
        {

            JArray jsonObjects = new JArray();
            var typeStr = data.GetType().FullName;

            if (typeStr == "Newtonsoft.Json.Linq.JArray")
            {
                var jarray = JArray.FromObject(data);
                jsonObjects = jarray;
            }
            else
            {
                var jobject = JObject.FromObject(data);
                jsonObjects.Add(data);
            }

            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
            var workUnitRepository = unitOfWork.GetRepository<WorkUnit>();
            var signUpCollectionRepository = unitOfWork.GetRepository<SignUpCollection>();

            var workUnit = workUnitRepository.Find(loginUnitId);
            SignUpForUnit signUpForUnit = null;
            if (signupforunitid != null)
            {
                signUpForUnit = signUpForUnitRepository.Query(c =>
                c.Id == (signupforunitid ?? 0) && c.WorkUnitId == loginUnitId
                , include: c => c.Include(s => s.SignUpCollection)).FirstOrDefault();
            }
            else
            {
                string code = DateTime.Now.ToLongTimeString() + workUnit.Id;
                signUpForUnit = new SignUpForUnit(code, loginUnitId, trainingcenterid, false, false);
            }
            if (signUpForUnit == null)
            {
                return BadRequest(new { message = "单位报名表错误", success = false });
            }

            foreach (var item in jsonObjects)
            {
                int studentid = (int)item["studentid"];
                int examsubjectid = (int)item["examsubjectid"];
                var signupCollectionisExists = signUpCollectionRepository.Query(c => c.StudentId == studentid && c.ExamSubjectId == examsubjectid)
                    .Include(s => s.Student).Include(s => s.ExamSubject)
                    .ToList();
                if (signupCollectionisExists.Count() > 0)
                {
                    return BadRequest(
                        new
                        {
                            message = signupCollectionisExists.Select(c => string.Format("存在已报名人员【{0}】【{1}】,修改已经撤销", c.Student.Name, c.ExamSubject.Name)),
                            success = false,
                            data = signupforunitid
                        });
                }
                signUpForUnit.AddSignUpCollection(studentid, examsubjectid);
            }

            var isExists = signUpForUnitRepository.Query(c => c.Code == signUpForUnit.Code && c.Id != signUpForUnit.Id).Any();
            if (isExists)
            {
                return BadRequest(new { message = "已经存在相同订单号的报名表", success = false, data = signupforunitid });
            }
            if (signUpForUnit.Id == 0)
            {
                context.SignUpForUnit.Add(signUpForUnit);
            }
            else
            {
                context.SignUpForUnit.Update(signUpForUnit);
            }
            context.SaveChanges();

            return Ok(new { message = "添加成功", success = true, data = signUpForUnit.Id });
        }
        [HttpPost]
        [Route("remove")]
        [Authorize]
        public IActionResult Remove([FromBody]Object data)
        {
            JArray jsonObjects = new JArray();
            var typeStr = data.GetType().FullName;

            if (typeStr == "Newtonsoft.Json.Linq.JArray")
            {
                var jarray = JArray.FromObject(data);
                jsonObjects = jarray;
            }
            else
            {
                var jobject = JObject.FromObject(data);
                jsonObjects.Add(data);
            }
            List<object> del = new List<object>();
            foreach (var item in jsonObjects)
            {
                del.Add((int)item["id"]);
            }
            var delarray = del.ToArray();
            IEnumerable<int> s = jsonObjects.Select(c => (int)c["id"]);
            var signUpCollectionRepository = unitOfWork.GetRepository<SignUpCollection>();
            var delobject = signUpCollectionRepository.Query(c => delarray.Contains(c.Id)).ToList();
            context.SignUpCollections.RemoveRange(delobject);
            context.SaveChanges();
            return Ok(new { message = "删除成功", success = true, data = delobject.FirstOrDefault().SignUpForUnitId });
        }
        [HttpPost]
        [Route("delete")]
        [Authorize]
        public IActionResult Delete([FromBody]Object data) {
            JArray jsonObjects = new JArray();
            var typeStr = data.GetType().FullName;

            if (typeStr == "Newtonsoft.Json.Linq.JArray")
            {
                var jarray = JArray.FromObject(data);
                jsonObjects = jarray;
            }
            else
            {
                var jobject = JObject.FromObject(data);
                jsonObjects.Add(data);
            }
            List<object> del = new List<object>();
            foreach (var item in jsonObjects)
            {
                del.Add((int)item["id"]);
            }
            var delarray = del.ToArray();
            IEnumerable<int> s = jsonObjects.Select(c => (int)c["id"]);
            var signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
            var delObje = signUpForUnitRepository.Query(c => s.Contains(c.Id),include:i=>i.Include(c=>c.SignUpCollection)).ToList();
            signUpForUnitRepository.Delete(delObje);
            unitOfWork.SaveChangesAsync();
            return Ok(new { message = "删除成功", success = true });
        }
        [HttpPost]
        [Route("readsignupforunit")]
        [Authorize]
        public async Task<IActionResult> ReadSignUpForUnit(int start, int limit, string filter, string query, string navigates)
        {
            var loginWorkUnitAccountId = int.Parse(User.FindFirstValue("AccountId"));
            var workUnitAccount = await context.WorkUnitAccounts
                .Include(s => s.WorkUnit).Where(c => c.Id == loginWorkUnitAccountId).FirstOrDefaultAsync();

            var search = context.SignUpForUnit
                .Include(s => s.WorkUnit)
                .Include(s => s.TrainingCenter)
                .Include(s => s.SignUpCollection)
                .Where(c => c.WorkUnitId == workUnitAccount.WorkUnitId);

            var filterObject = filter.ToObject<Filter>().FirstOrDefault();
            if (filterObject != null)
            {
                string value = filterObject.Value;
                search = search.Where(c => c.Code.Contains(value));
            }

            var items = search.Skip(start).Take(limit);
            var result = items.ToList().Select(c => new Dictionary<string, object>
                {
                    {"id",c.Id},
                    {"workunit.name",c.WorkUnit.Name },
                    {"workunit.id",c.WorkUnitId},
                    {"trainingcenter.name",c.TrainingCenter.Name},
                    {"trainingcenter.id",c.TrainingCenterId },
                    {"ispay",c.IsPay},
                    {"islock",c.IsLock},
                    {"code",c.Code },
                    {"studentcount",c.SignUpCollection.Count() },
                    {"createtime",c.CreateTime }
                });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
        [Route("export")]
        public IActionResult Export()
        {
            ExcelHelper.ExcelTest();
            return Ok();
        }

    }


}

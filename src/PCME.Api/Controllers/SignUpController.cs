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
        private readonly IRepository<SignUpForUnit> signUpForUnitRepository;
        private readonly IRepository<SignUpCollection> signUpCollectionRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<WorkUnit> workUnitRepository;
        private readonly IRepository<WorkUnitAccount> workUnitAccountRepository;
        private readonly IRepository<ExamSubject> examSubjectRepository;
        private readonly IRepository<ExamSubjectOpenInfo> examSubjectOpenInfoRepository;
        public SignUpController(IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext context)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            signUpForUnitRepository = this.unitOfWork.GetRepository<SignUpForUnit>();
            signUpCollectionRepository = this.unitOfWork.GetRepository<SignUpCollection>();
            studentRepository = this.unitOfWork.GetRepository<Student>();
            workUnitRepository = this.unitOfWork.GetRepository<WorkUnit>();
            workUnitAccountRepository = this.unitOfWork.GetRepository<WorkUnitAccount>();
            examSubjectRepository = this.unitOfWork.GetRepository<ExamSubject>();
            examSubjectOpenInfoRepository = this.unitOfWork.GetRepository<ExamSubjectOpenInfo>();
        }
        /// <summary>
        /// 读取本单位报名人员列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readstudentlist")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadStudentList(int start, int limit, string filter, string query, string navigates)
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

        /// <summary>
        /// 读取当前可以表报名的培训点列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readtrainingcenterlist")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadTrainingCenterList(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var loginAccountId = int.Parse(User.FindFirstValue("AccountId"));

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
            var search2 = from c in search.ToList()
                          group c by c.TrainingCenter into g
                          select new { g.Key.Id,g.Key.Name,g.Key.Address, OpenTypeId = g.Key.OpenType.Name, OpenTypeName = g.Key.OpenType.Name };
            

            var item = search2.Skip(start).Take(limit);
            var result = item.ToList().Select(c => new Dictionary<string, object>
                {
                    { "id",c.Id},
                    { "name",c.Name},
                    { "address",c.Address},
                    { "OpenType.Id",c.OpenTypeId},
                    { "OpenType.Name",c.OpenTypeName}
                });
            var total = search2.Count();
            return Ok(new { total, data = result });
        }

        /// <summary>
        /// 根据显示单位报名表详情列表（单位报名表的人员列表）
        /// </summary>
        /// <param name="signupforunitid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readsignupcollection")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadSignUpCollection(int? signupforunitid)
        {
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

        /// <summary>
        /// 读取指定培训点开设的表报名科目
        /// </summary>
        /// <param name="trainingId"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readexamsubjectopeninfo")]
        [Authorize(Roles = "Unit")]
        public IActionResult ReadExamSubjectOpenInfo(int trainingId, int start, int limit, string filter, string query, string navigates)
        {
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
        /// <summary>
        /// 创建或修改报名表
        /// </summary>
        /// <param name="signupforunitid"></param>
        /// <param name="trainingcenterid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Unit")]
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
                var count = signUpForUnitRepository.Query(c => c.WorkUnitId == loginUnitId).Count();
                string code = DateTime.Now.ToString("yyMMddHHssmm") + (count + 1).ToString(); //DateTime.Now.ToLongTimeString() + workUnit.Id;
                signUpForUnit = new SignUpForUnit(code, loginUnitId, trainingcenterid, false, false);
            }
            if (signUpForUnit == null)
            {
                return BadRequest(new { message = "单位报名表错误", success = false });
            }
            if (signUpForUnit.IsPay)
            {
                return BadRequest(new { message = "已经扫描成功的报名表不允许编辑", success = false, data = signupforunitid });
            }


            List<dynamic> badRequest = new List<dynamic>();

            foreach (var item in jsonObjects)
            {
                int studentid = (int)item["studentid"];
                int examsubjectid = (int)item["examsubjectid"];
                var signupCollectionisExists = signUpCollectionRepository.Query(c => c.StudentId == studentid && c.ExamSubjectId == examsubjectid)
                    .Include(s => s.Student).Include(s => s.ExamSubject)
                    .FirstOrDefault();

                var signupisExists_query = from signup in context.SignUp
                                           join students in context.Students on signup.StudentId equals students.Id into left1
                                           from students in left1.DefaultIfEmpty()
                                           join examsubjects in context.ExamSubjects on signup.ExamSubjectId equals examsubjects.Id into left2
                                           from examsubjects in left2.DefaultIfEmpty()
                                           where signup.StudentId == studentid && signup.ExamSubjectId == examsubjectid
                                           select new { signup, students, examsubjects };
                if (signupisExists_query.Any())
                {
                    var existsFirst = signupisExists_query.FirstOrDefault();
                    badRequest.Add(
                       new
                       {
                           message = string.Format("存在已 【报名成功】 人员【{0}】【{1}】,修改已经撤销", existsFirst.students.Name, existsFirst.examsubjects.Name),
                           signupforunitid,
                           examsubjectid = existsFirst.examsubjects.Id,
                           studentid = existsFirst.students.Id,
                           studentname = existsFirst.students.Name,
                           examsubjectname = existsFirst.examsubjects.Name,
                           idcard = existsFirst.students.IDCard
                       });
                }

                var creditexamisExists_query = from creditexams in context.CreditExams
                                               join students in context.Students on creditexams.StudentId equals students.Id into left1
                                               from students in left1.DefaultIfEmpty()
                                               join examsubjects in context.ExamSubjects on creditexams.SubjectId equals examsubjects.Id into left2
                                               from examsubjects in left2.DefaultIfEmpty()
                                               where creditexams.StudentId == studentid && creditexams.SubjectId == examsubjectid
                                               select new { creditexams, students, examsubjects };
                if (creditexamisExists_query.Any())
                {
                    var existsFirst = creditexamisExists_query.FirstOrDefault();
                    badRequest.Add(
                       new
                       {
                           message = string.Format("存在已 【合格】 人员【{0}】【{1}】,修改已经撤销", existsFirst.students.Name, existsFirst.examsubjects.Name),
                           signupforunitid,
                           examsubjectid = existsFirst.examsubjects.Id,
                           studentid = existsFirst.students.Id,
                           studentname = existsFirst.students.Name,
                           examsubjectname = existsFirst.examsubjects.Name,
                           idcard = existsFirst.students.IDCard
                       });
                }

                if (signupCollectionisExists != null)
                {
                    badRequest.Add(
                        new
                        {
                            message= string.Format("已有【报名表中】中存在已 【报名】 人员【{0}】【{1}】,修改已经撤销",signupCollectionisExists.Student.Name,signupCollectionisExists.ExamSubject.Name),
                            signupforunitid = signupCollectionisExists.SignUpForUnitId,
                            examsubjectid = signupCollectionisExists.ExamSubjectId,
                            studentid = signupCollectionisExists.StudentId,
                            studentname = signupCollectionisExists.Student.Name,
                            examsubjectname = signupCollectionisExists.ExamSubject.Name,
                            idcard = signupCollectionisExists.Student.IDCard
                        });
                }
                else { signUpForUnit.AddSignUpCollection(studentid, examsubjectid); }
            }
            if (badRequest.Any())
            {
                return BadRequest(
                    new
                    {
                        badRequest.FirstOrDefault().message,
                        success = false,
                        data = badRequest
                    });
            }

            var isExists = signUpForUnitRepository.Query(c => c.Code == signUpForUnit.Code && c.Id != signUpForUnit.Id).Any();
            if (isExists)
            {
                return BadRequest(new { message = "已经存在相同编号的报名表", success = false, data = signupforunitid });
            }

            if (!signUpForUnit.SignUpCollection.Any())
            {
                return BadRequest(new { message = "您不能申报一张空报名表", success = false, data = signupforunitid });
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
        /// <summary>
        /// 删除报名表详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Unit")]
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
            var delobject = signUpCollectionRepository.Query(c => delarray.Contains(c.Id)).ToList();

            var signUpForUnit = signUpForUnitRepository.Find(delobject.FirstOrDefault().SignUpForUnitId);
            if (signUpForUnit.IsPay)
            {
                return BadRequest(new { message = "已经扫描成功的报名表不允许编辑", success = false, data = delobject.FirstOrDefault().SignUpForUnitId });
            }

            context.SignUpCollections.RemoveRange(delobject);
            context.SaveChanges();
            return Ok(new { message = "删除成功", success = true, data = delobject.FirstOrDefault().SignUpForUnitId });
        }

        /// <summary>
        /// 删除报名表并和报名表人员
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [Authorize(Roles = "Unit")]
        public IActionResult Delete([FromBody]Object data)
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

            var delObje = signUpForUnitRepository.Query(c => s.Contains(c.Id)).FirstOrDefault();

            if (delObje.IsPay)
            {
                return BadRequest(new { message = "删除失败，已经扫描成功的把报名表不允许删除", success = false });
            }

            var delcollections = signUpCollectionRepository.Query(c => c.SignUpForUnitId == delObje.Id).ToList();
            signUpCollectionRepository.Delete(delcollections);
            signUpForUnitRepository.Delete(delObje);
            unitOfWork.SaveChanges();
            return Ok(new { message = "删除成功", success = true });
        }

        /// <summary>
        /// 读取当前单位的报名表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readsignupforunit")]
        [Authorize(Roles = "Unit")]
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
        /// <summary>
        /// 报名表到处Excel 未完成
        /// </summary>
        /// <returns></returns>
        [Route("export")]
        [Authorize(Roles = "Unit")]
        public IActionResult Export()
        {
            ExcelHelper.ExcelTest();
            return Ok();
        }
        /// <summary>
        /// 单位锁定报名按钮
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveorupdatesignupforunit")]
        [Authorize(Roles = "Unit")]
        public IActionResult SaveOrUpdate([FromBody]JToken data)
        {
            //int s = int.TryParse(data.islock);
            int key = (int)data["id"];
            bool islock = (bool)data["islock"];
            var signUpForUnit = signUpForUnitRepository.Find(key);
            signUpForUnit.ChangeIsLock(islock);
            signUpForUnitRepository.Update(signUpForUnit);
            unitOfWork.SaveChanges();
            return Ok(new { message = "锁定成功", success = true });
        }
        
    }


}

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
using Newtonsoft.Json.Linq;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.ApplicationForm;

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
        private readonly IRepository<WorkUnit> workUnitRepository;

        public ApplyController(ApplicationDbContext context, IMediator _mediator,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            this._mediator = _mediator;
            this.workUnitAccountRepository = this.unitOfWork.GetRepository<WorkUnitAccount>();
            this.studentRepository = this.unitOfWork.GetRepository<Student>();
            this.workUnitRepository = this.unitOfWork.GetRepository<WorkUnit>();
        }

        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> Read(int start, int limit, string filter, string query, string navigates)
        {

            var search = context.ApplyForSetting.Where(c=>DateTime.Now >= c.StartTime && DateTime.Now <= c.EndTime);
            var examsubjectItem = await context.ExamSubjects.ToListAsync();

            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var result = search.ToList().Select(c => new Dictionary<string, object>
            {
                {"id",c.Id},
                {"title",c.Title},
                {"examSubjectNameItem",string.Join(',',examsubjectItem.Where(w=>c.SubjectIds.Contains(w.Id)).Select(s=>s.Name))},
                {"examSbuejctIdItems",c.ExamSubjectIdString},
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


            var queryExamCredit = from q1 in search
                                  let examsubjectIdItems = context.CreditExams.Where(w => w.StudentId == q1.Id).Select(s=>s.SubjectId)
                                  select new { q1,examsubjectIdItems };


            var item = queryExamCredit.Skip(start).Take(limit);


            var result = item.Select(c => new Dictionary<string, object> {
                { "id",c.q1.Id},
                {"examsubjectIdItems",c.examsubjectIdItems},
                { "name",c.q1.Name},
                { "idcard",c.q1.IDCard},
                { "officename",c.q1.OfficeName},
                { "password",c.q1.Password},
                { "Sex.Id",c.q1.SexId},
                { "Sex.Name",c.q1.Sex.Name},
                { "StudentType.Id",c.q1.StudentTypeId},
                { "StudentType.Name",c.q1.StudentType.Name},
                { "StudentStatus.Id",c.q1.StudentStatusId},
                { "StudentStatus.Name",c.q1.StudentStatus.Name},
                { "graduationschool",c.q1.GraduationSchool},
                { "specialty",c.q1.Specialty},
                { "workdate",c.q1.WorkDate},
                { "WorkUnit.Id",c.q1.WorkUnitId},
                { "WorkUnit.Name",c.q1.WorkUnit.Name},
                { "address",c.q1.Address},
                { "email",c.q1.Email},
                { "birthday",c.q1.BirthDay}
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

            var search = context.StudentItem.Include(i => i.Student).Where(c => c.ApplyTableId == applyTableId);
            if (search == null)
            {
                return Ok(new { total=0, data = "" });
            }
            //var item = search.Skip(start).Take(limit);
            var result = search.ToList().Select(c => new Dictionary<string, object>
                {
                    { "id",c.Id},
                    { "studentidcard",c.Student?.IDCard},
                    { "studentname",c.Student?.Name},
                    { "studentid",c.Student?.Id}
                });
            var total = search.Count();
            return Ok(new { total, data = result });
        }


        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Unit")]
        public IActionResult SaveOrUpdate([FromQuery]int? applyTableid,[FromQuery]int applyforsettingid, [FromBody]Object data)
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
            ApplyTable applyTable = null;
            if (applyTableid != null) //如果存在id则是编辑申请表 则先读出申请表
            {
                applyTable = context.ApplyTable.Where(c => c.Id == (applyTableid ?? 0) && c.WorkUnitId == loginUnitId).Include(i => i.StudentItems).FirstOrDefault();
            }
            else
            {
                var count = context.ApplyTable.Where(c => c.WorkUnitId == loginUnitId).Count();
                string code = "APPLY"+DateTime.Now.ToString("yyMMddHHssmm") + (count + 1).ToString(); //DateTime.Now.ToLongTimeString() + workUnit.Id;
                applyTable = new ApplyTable(loginUnitId, DateTime.Now, code,applyforsettingid); //(code, loginUnitId, trainingcenterid, false, false);
            }
            if (applyTable == null)
            {
                return BadRequest(new { message = "单位申请表错误", success = false });
            }
            if (signUpForUnit.IsPay)
            {
                return BadRequest(new { message = "已经扫描成功的报名表不允许编辑", success = false, data = signupforunitid });
            }


            List<dynamic> badRequest = new List<dynamic>();

            foreach (var item in jsonObjects)
            {
                int studentid = (int)item["studentid"];
                var studentItemIsExists = context.StudentItem.Include(s=>s.ApplyTable).Include(s=>s.Student).Where(c => c.StudentId == studentid && c.ApplyTable.ApplyForSettingId == applyforsettingid).FirstOrDefault();
                if (studentItemIsExists != null)
                {
                    badRequest.Add(
                       new
                       {
                           message = string.Format("存在已 【申请成功】 人员【{0}】,修改已经撤销", studentItemIsExists.Student.Name),
                           applyforsettingid,
                           studentid = studentItemIsExists.Student.Id,
                           studentname = studentItemIsExists.Student.Name,
                           idcard = studentItemIsExists.Student.IDCard
                       });
                }
                else { applyTable.AddStudentItem(studentid); }
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

            var isExists = context.ApplyTable.Where(c => c.Num == applyTable.Num && c.Id != applyTable.Id).Any(); //signUpForUnitRepository.Query(c => c.Code == signUpForUnit.Code && c.Id != signUpForUnit.Id).Any();
            if (isExists)
            {
                return BadRequest(new { message = "已经存在相同编号的申请表", success = false, data = applyTableid });
            }

            if (!applyTable.StudentItems.Any())
            {
                return BadRequest(new { message = "您不能申报一张空申请表", success = false, data = applyTableid });
            }

            if (applyTable.Id == 0)
            {
                context.ApplyTable.Add(applyTable);
            }
            else
            {
                context.ApplyTable.Update(applyTable);
            }
            context.SaveChanges();

            return Ok(new { message = "添加成功", success = true, data = applyTable.Id });
        }

        /// <summary>
        /// 读取当前单位的申请表
        /// </summary>
        /// <returns>The sign up for unit.</returns>
        /// <param name="start">Start.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="navigates">Navigates.</param>
        [HttpPost]
        [Route("readapplytable")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> ReadApplyTable(int start, int limit, string filter, string query, string navigates)
        {
            var loginWorkUnitAccountId = int.Parse(User.FindFirstValue("AccountId"));
            var workUnitAccount = await context.WorkUnitAccounts
                .Include(s => s.WorkUnit).Where(c => c.Id == loginWorkUnitAccountId).FirstOrDefaultAsync();

            var search = context.ApplyTable
                .Include(s => s.WorkUnit)
                .Include(s => s.StudentItems)
                .Include(s=>s.ApplyForSetting)
                .Where(c => c.WorkUnitId == workUnitAccount.WorkUnitId);

            var filterObject = filter.ToObject<Filter>().FirstOrDefault();
            if (filterObject != null)
            {
                string value = filterObject.Value;
                search = search.Where(c => c.Num.Contains(value));
            }

            var items = search.Skip(start).Take(limit);
            //var test = items.ToList();
            var result = items.ToList().Select(c => new Dictionary<string, object>
                {
                    {"id",c.Id},
                    {"workunit.name",c.WorkUnit.Name },
                    {"workunit.id",c.WorkUnitId},
                    {"applyforsetting.title",c.ApplyForSetting.Title},
                    {"applyforsetting.id",c.ApplyForSetting.Id },
                    {"examSbuejctIdItems",c.ApplyForSetting.ExamSubjectIdString },
                    {"ispay",c.IsPay},
                    {"islock",c.IsLock},
                    {"num",c.Num },
                    {"studentcount",c.StudentItems.Count() },
                    {"createtime",c.CreateTime }
                });
            var total = search.Count();
            return Ok(new { total, data = result });
        }

        /// <summary>
        /// 删除申请表并和申请表人员
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

            var delObje = context.ApplyTable.Where(c => s.Contains(c.Id)).FirstOrDefault();// signUpForUnitRepository.Query(c => s.Contains(c.Id)).FirstOrDefault();

            if (delObje.IsPay)
            {
                return BadRequest(new { message = "删除失败，已经扫描成功的把报名表不允许删除", success = false });
            }

            var delcollections = context.StudentItem.Where(c => c.ApplyTableId == delObje.Id).ToList(); // signUpCollectionRepository.Query(c => c.SignUpForUnitId == delObje.Id).ToList();
            context.StudentItem.RemoveRange(delcollections);
            context.ApplyTable.Remove(delObje);
            unitOfWork.SaveChanges();
            return Ok(new { message = "删除成功", success = true });
        }

        /// <summary>
        /// 单位锁定申请按钮
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveorupdateapplytable")]
        [Authorize(Roles = "Unit")]
        public IActionResult SaveOrUpdateApplyTable([FromBody]JToken data)
        {
            //int s = int.TryParse(data.islock);
            int key = (int)data["id"];
            bool islock = (bool)data["islock"];
            var applyTable = context.ApplyTable.Find(key);
            applyTable.ChangeIsLock(islock);
            context.ApplyTable.Update(applyTable);
            context.SaveChanges();
            return Ok(new { message = "锁定成功", success = true });
        }
    }
}

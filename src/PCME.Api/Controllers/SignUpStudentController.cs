using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/SignUpStudent")]
    public class SignUpStudentController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;
        public SignUpStudentController(IMediator mediator, ApplicationDbContext context)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <summary>
        /// 读取当前可以表报名的培训点列表(个人报名)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <param name="navigates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readtrainingcenterlist")]
        [Authorize(Roles = "Student")]
        public IActionResult ReadTrainingCenterListByStudent(int start, int limit, string filter, string query, string navigates)
        {
            //var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var studentid = int.Parse(User.FindFirstValue("AccountId"));

            var student = context.Students.Find(studentid);
            OpenType type = OpenType.Professional;

            if (student.StudentTypeId == StudentType.CivilServant.Id)
            {
                type = OpenType.CivilServant;
            }
            else
            {
                type = OpenType.Professional;
            }

            var search = context.ExamSubjectOpenInfo.Where(c =>
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
                          select new { g.Key.Id, g.Key.Name, g.Key.Address, OpenTypeId = g.Key.OpenType.Name, OpenTypeName = g.Key.OpenType.Name };


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


        [HttpPost]
        [Route("readexamsubjectopeninfo")]
        [Authorize(Roles = "Student")]
        public IActionResult ReadExamSubjectOpenInfo(int trainingId, int start, int limit, string filter, string query, string navigates)
        {
            var search = context.ExamSubjectOpenInfo.Where(c =>
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
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult Read(int studentsignupid, int start, int limit, string filter, string query, string navigates)
        {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            
            var search = from signupstudent in context.SignUpStudent.Include(s => s.Collection)
                        join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                        join student in context.Students on signupstudent.StudentId equals student.Id
                        where signupstudent.StudentId == studentid
                        select new { signupstudent, trainingcenter, student };
            if (studentsignupid > 0)
            {
                search = search.Where(c => c.signupstudent.Id == studentsignupid);
            }
            var items = search.Skip(start).Take(limit);
            var result = items.Select(c => new Dictionary<string, object>
            {
                {"id",c.signupstudent.Id},
                {"Collection",c.signupstudent.Collection.Join(
                    context.ExamSubjects,
                    l=>l.ExamSubjectId,
                    r=>r.Id,
                    (l,r)=>new { l,r}
                    ).Select(s=>new Dictionary<string,object>{
                    {"examsubjectid",s.l.ExamSubjectId},
                    {"examsubjectname",s.r.Name}
                })},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"student.Id",c.student.Id},
                {"student.Name",c.student.Name},
                {"student.IDCard",c.student.IDCard}
            });
            return Ok(new { total = search.Count(), data = result});
        }
        [HttpPost]
        [Route("readcollection")]
        [Authorize(Roles = "Student")]
        public IActionResult ReadCollection(int studentsignupid, int start, int limit, string filter, string query, string navigates)
        {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            var search = from signupstudentcollection in context.SignUpStudentCollection
                        join signupstudent in context.SignUpStudent on signupstudentcollection.SignUpStudentId equals signupstudent.Id
                        join examsubject in context.ExamSubjects on signupstudentcollection.ExamSubjectId equals examsubject.Id
                        join student in context.Students on signupstudent.StudentId equals student.Id
                        join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                        where signupstudent.StudentId == studentid
                        orderby signupstudent.CreateTime descending
                        select new { signupstudentcollection, signupstudent, examsubject, student, trainingcenter };

            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            if (studentsignupid > 0)
            {
                search = search.Where(c => c.signupstudent.Id == studentsignupid);
            }
            var items = search.Skip(start).Take(limit);
            var result = items.Select(c => new Dictionary<string, object>
            {
                {"id",c.signupstudentcollection.Id},
                {"signupstudent.Code",c.signupstudent.Code},
                {"signupstudent.Id",c.signupstudent.Id},
                {"examsubject.Id",c.examsubject.Id},
                {"examsubject.Name",c.examsubject.Name},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"student.Id",c.student.Id},
                {"student.Name",c.student.Name},
                {"student.IDCard",c.student.IDCard}
            });
            return Ok(new { total = search.Count(), data = result });
        }
        [HttpPost]
        [Route("savesignupstudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Save([FromBody]SignUpStudentCreateOrUpdateCommand command)
        {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            command.Id = 0;
            command.Code = DateTime.Now.ToString("yyyyMMdd") + studentid + command.TrainingCenterId  + DateTime.Now.ToString("HHssmm");
            command.StudentId = studentid;
            var isExists = context.SignUpStudentCollection.Include(s => s.SignUpStudent).Where(c =>
              c.SignUpStudent.StudentId == studentid && command.CollectionDTO.Any(a => a.ExamSubjectId == c.ExamSubjectId)
            ).Join(context.ExamSubjects, l => l.ExamSubjectId, r => r.Id, (l, r) => new { l, r });
            List<dynamic> badRequest = new List<dynamic>();
            if (isExists.Any())//检测报名表中是否存在报名
            {
                await isExists.ForEachAsync(c =>
                        badRequest.Add(
                            new
                            {
                                message = string.Format("报名表【{0}】中【{1}】科目已经存在。",
                                c.l.SignUpStudent.Code,
                                c.r.Name
                                ),
                                examsubjectid = c.r.Id
                            }
                            )
                    );
            }
            var isExistsSignUp = context.SignUp.Where(c => c.StudentId == studentid && command.CollectionDTO.Any(a => a.ExamSubjectId == c.ExamSubjectId))
                .Join(context.ExamSubjects,l=>l.ExamSubjectId,r=>r.Id,(l,r)=>new { l,r});
            if (isExistsSignUp.Any())
            {
                await isExistsSignUp.ForEachAsync(c =>
                        badRequest.Add(
                            new
                            {
                                message = string.Format("【{0}】科目已经正式报名成功。",
                                c.r.Name
                                ),
                                examsubjectid = c.r.Id
                            }
                            )
                    );
            }

            var isExistsSignUpForUnitCollectioni = context.SignUpCollections.Include(s=>s.SignUpForUnit).Where(c=>c.StudentId == studentid && command.CollectionDTO.Any(a=>a.ExamSubjectId == c.ExamSubjectId))
                .Join(context.ExamSubjects, l => l.ExamSubjectId, r => r.Id, (l, r) => new { l, r });
            if (isExistsSignUpForUnitCollectioni.Any())
            {
                await isExistsSignUpForUnitCollectioni.ForEachAsync(c =>
                        badRequest.Add(
                            new
                            {
                                message = string.Format("【{0}】科目已经在单位【{1}】报名表中。",
                                c.r.Name,
                                c.l.SignUpForUnit.Code
                                ),
                                examsubjectid = c.r.Id
                            }
                            )
                    );
            }

            if (badRequest.Any())
            {
                return BadRequest(badRequest);
            }

            var result = await mediator.Send(command);
            return Ok(new { success=true});
        }
    }
}

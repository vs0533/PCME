using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/ExamSubjectOpenInfo")]
	public class ExamSubjectOpenInfoController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
		private readonly IRepository<ExamSubjectOpenInfo> examSubjectOpenInfoRepository;
		private readonly IRepository<TrainingCenter> trainingCenterRepository;
		public ExamSubjectOpenInfoController(IUnitOfWork<ApplicationDbContext> unitOfWork,IMediator _mediator)
		{
			this.unitOfWork = unitOfWork;
			examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
			trainingCenterRepository = unitOfWork.GetRepository<TrainingCenter>();
			this._mediator = _mediator;
		}

		[HttpPost]
		[Route("read")]
		[Authorize(Roles = "TrainingCenter")]
		public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
		{
			var trainingId = int.Parse(User.FindFirstValue("WorkUnitId").ToString());
			var search = examSubjectOpenInfoRepository.Query(c => c.TrainingCenterId == trainingId)
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
				    {"displayexamtime",c.DisplayExamTime},
                    {"gotovaldatetime",c.GoToValDateTime},
                    {"pirce",c.Pirce}
                });
			var total = search.Count();
			return Ok(new { total, data = result });
		}

		[HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "TrainingCenter")]
		public async Task<IActionResult> Post([FromBody]ExamSubjectOpenInfoCreateOrUpdateCommand command, string opertype)
        {
			var trainingId = int.Parse(User.FindFirstValue("WorkUnitId").ToString());         
			var loginobj = await trainingCenterRepository.FindAsync(trainingId);
			if (opertype == "new")
            {
                command.SetId(0);
            }
			command.SetTrainingCenter(trainingId);
			command.SetAuditStatus(AuditStatus.Wait.Id);
            

			var examSubjectExisted = examSubjectOpenInfoRepository.GetFirstOrDefault(predicate: c =>
			     (c.TrainingCenterId == command.TrainingCenterId && c.Id != command.Id) && c.ExamSubjectId == command.ExamSubjectId
            );
			
			if (examSubjectExisted != null)
            {
				ModelState.AddModelError("ExamSubject.Id", "相同科目的申请已经存在，请前去修改");
            }

			if (command.SignUpFinishTime <= command.SignUpTime)
			{
				ModelState.AddModelError("signupfinishtime", "报名截止时间不得小于开始时间");
			}

			ModelState.Remove("opertype");

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
					{"id",result.Id},
					{"AuditStatus.Id",result.AuditStatusId},
					{"AuditStatus.Name",result.AuditStatus.Name},
					{"ExamSubject.Id",result.ExamSubjectId},
					{"ExamSubject.Name",result.ExamSubject.Name},
					{"TrainingCenter.Name",result.TrainingCenter.Name},
					{"TrainingCenter.Id",result.TrainingCenterId},
					{"signuptime",result.SignUpTime},
					{"signupfinishtime",result.SignUpFinishTime},
					{"signupfinishoffset",result.SignUpFinishOffset},
					{"displayexamtime",result.DisplayExamTime},
                    {"gotovaldatetime",result.GoToValDateTime},
                    {"pirce",result.Pirce}
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();

        }

		[HttpPost]
        [Route("remove")]
		[Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

			var delExamSubjectOpenInfo = await examSubjectOpenInfoRepository.FindAsync(id);
			if (delExamSubjectOpenInfo is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }

			if (delExamSubjectOpenInfo.AuditStatusId == AuditStatus.Pass.Id)
			{
				return Ok(new { message = "审核通过的科目不允许删除" });
			}

			examSubjectOpenInfoRepository.Delete(delExamSubjectOpenInfo);
            await unitOfWork.SaveEntitiesAsync();
			return NoContent();
        }
	}
}

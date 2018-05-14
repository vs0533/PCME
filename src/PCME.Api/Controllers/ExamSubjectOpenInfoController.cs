using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/ExamSubjectOpenInfo")]
	public class ExamSubjectOpenInfoController : Controller
	{
		private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
		private readonly IRepository<ExamSubjectOpenInfo> examSubjectOpenInfoRepository;
		public ExamSubjectOpenInfoController(IUnitOfWork<ApplicationDbContext> unitOfWork)
		{
			this.unitOfWork = unitOfWork;
			examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
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
				    {"displayexamtime",c.DisplayExamTime}
				});
			var total = search.Count();
			return Ok(new { total, data = result });
		}
	}
}

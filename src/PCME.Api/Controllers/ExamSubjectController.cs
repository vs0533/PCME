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
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/ExamSubject")]
	public class ExamSubjectController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
		private readonly IRepository<ExamSubject> examsubjectRepository;
		private readonly IRepository<Series> seriesRepository;
		private readonly ApplicationDbContext _dbContext;
		private readonly IRepository<ExamSubjectOpenInfo> examSubjectOpenInfoRepository;

		public ExamSubjectController(ApplicationDbContext _dbContext, IMediator mediator, IUnitOfWork<ApplicationDbContext> unitOfWork)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this._dbContext = _dbContext;
			seriesRepository = unitOfWork.GetRepository<Series>();
			examsubjectRepository = unitOfWork.GetRepository<ExamSubject>();
			examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
		}

		[HttpPost]
		[Route("fetchinfo")]
		public IActionResult FindById(int id)
		{
			var query = examsubjectRepository.Query(predicate: c => c.Id == id)
			                                        .Include(s => s.OpenType)
                                                    .Include(s => s.ExamType)
                                                    .Include(s => s.ExamSubjectStatus)
			                                        .Include(s => s.Series).FirstOrDefault();
			if (id <= 0)
			{
				return BadRequest();
			}

			if (query != null)
			{
				var result = new Dictionary<string, object>{
					{"id",query.Id},
					{"name",query.Name},
					{"code",query.Code},
					{"credithour",query.CreditHour},
					{"ExamSubjectStatus.Id",query.ExamSubjectStatusId},
					{"ExamSubjectStatus.Name",query.ExamSubjectStatus?.Name},
					{"ExamType.Id",query.ExamTypeId},
					{"ExamType.Name",query.ExamType.Name},
					{"OpenType.Id",query.OpenTypeId},
					{"OpenType.Name",query.OpenType.Name},
					{"Series.Id",query.SeriesId},
					{"Series.Name",query.Series?.Name},
					{"mscount",query.MSCount}
				};
				return Ok(new { data = result });
			}

			return NotFound();
		}

		[HttpGet]
        [Route("read")]
        public IActionResult StoreRead()
        {
			int trainingId = 0;
			try
			{
				trainingId = int.Parse(User.FindFirstValue("WorkUnitId").ToString());
			}
			catch(Exception)
			{
				trainingId = 0;
			}

			var search = examsubjectRepository.Query(c=>c.ExamSubjectStatusId == ExamSubjectStatus.Default.Id);
            if (trainingId != 0)
            {
				var isExists = examSubjectOpenInfoRepository.Query(c => c.TrainingCenterId == trainingId).Select(c=>c.Id);
				search = search.Where(c => isExists.Contains(c.Id) != true);
            }

            return Ok(search.Select(c => new { value = c.Id, text = c.Name }));
        }

		[HttpPost]
		[Route("read")]
		[Authorize(Roles = "Admin")]
		public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
		{
			var search = examsubjectRepository.Query(c => c.Id != 0)
											  .Include(s => s.OpenType)
											  .Include(s => s.ExamType)
											  .Include(s => s.ExamSubjectStatus)
											  .Include(s => s.Series)
			                                  .FilterAnd(filter.ToObject<Filter>())
                                              .FilterOr(query.ToObject<Filter>());;


			var item = search.Skip(start).Take(limit);
			var result = item.ToList().Select(c => new Dictionary<string, object>
				{
					{"id",c.Id},
					{"name",c.Name},
					{"code",c.Code},
					{"credithour",c.CreditHour},
					{"ExamSubjectStatus.Id",c.ExamSubjectStatusId},
					{"ExamSubjectStatus.Name",c.ExamSubjectStatus?.Name},
					{"ExamType.Id",c.ExamTypeId},
					{"ExamType.Name",c.ExamType.Name},
					{"OpenType.Id",c.OpenTypeId},
					{"OpenType.Name",c.OpenType.Name},
					{"Series.Id",c.SeriesId},
					{"Series.Name",c.Series?.Name},
					{"mscount",c.MSCount}
				});
			var total = search.Count();
			return Ok(new { total, data = result });
		}
		[HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]ExamSubjectCreateOrUpdateCommand command, string opertype)
        {
            var loginid = int.Parse(User.FindFirstValue("WorkUnitId"));

			var codeExisted = examsubjectRepository.GetFirstOrDefault(predicate: c =>
                 c.Code == command.Code && c.Id != command.Id
            );
			var nameExistend = examsubjectRepository.GetFirstOrDefault(predicate: c =>
                c.Name == command.Name && c.Id != command.Id
            );
			if (codeExisted != null)
            {
                ModelState.AddModelError("code", "相同代码的科目已经存在");
            }
			if (nameExistend != null)
            {
                ModelState.AddModelError("name", "相同名称的科目已经存在");
            }
            ModelState.Remove("opertype");
            
            if (ModelState.IsValid)
            {
                ExamSubject result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
					{"id",result.Id},
					{"name",result.Name},
					{"code",result.Code},
					{"credithour",result.CreditHour},
					{"ExamSubjectStatus.Id",result.ExamSubjectStatusId},
					{"ExamSubjectStatus.Name",result.ExamSubjectStatus?.Name},
					{"ExamType.Id",result.ExamTypeId},
					{"ExamType.Name",result.ExamType.Name},
					{"OpenType.Id",result.OpenTypeId},
					{"OpenType.Name",result.OpenType.Name},
					{"Series.Id",result.SeriesId},
					{"Series.Name",result.Series?.Name},
					{"mscount",result.MSCount}
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();         
        }

		[HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

			var delExamSubject = await examsubjectRepository.FindAsync(id);
			if (delExamSubject is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }

			delExamSubject.ChangeExamSubjectStatus(ExamSubjectStatus.Forbidden.Id);
			examsubjectRepository.Update(delExamSubject);
            await unitOfWork.SaveEntitiesAsync();
			return Ok(new { message = "科目被已被设为禁用" });
        }
	}
}

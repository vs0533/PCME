using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
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

		public ExamSubjectController(ApplicationDbContext _dbContext, IMediator mediator, IUnitOfWork<ApplicationDbContext> unitOfWork)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this._dbContext = _dbContext;
			seriesRepository = unitOfWork.GetRepository<Series>();
			examsubjectRepository = unitOfWork.GetRepository<ExamSubject>();
		}

		[HttpPost]
		[Route("fetchinfo")]
		public async Task<IActionResult> FindById(int id)
		{
			var query = await examsubjectRepository.GetFirstOrDefaultAsync(predicate: c => c.Id == id);
			if (id <= 0)
			{
				return BadRequest();
			}

			if (query != null)
			{
				var result = new Dictionary<string, object>{
					{ "id",query.Id},
					{ "name",query.Name}
				};
				return Ok(new { data = result });
			}

			return NotFound();
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
	}
}

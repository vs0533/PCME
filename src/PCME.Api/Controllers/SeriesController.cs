using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
	[Produces("application/json")]
    [Route("api/Series")]
    [Authorize]
	public class SeriesController:Controller
    {

		readonly IRepository<Series> seriesRepository;
		readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
		public SeriesController(IUnitOfWork<ApplicationDbContext> unitOfWork)
		{
			this.unitOfWork = unitOfWork;
			seriesRepository = unitOfWork.GetRepository<Series>();
		}

		[HttpPost]
		[Route("read")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
			var search = seriesRepository.Query(c=>c.Id != 0);


            var result = search.Select(c => new Dictionary<string, object> {
                {"id",c.Id},
                {"name",c.Name}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
        
		[HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
			return Ok(new { data = seriesRepository.FindAsync(id) });
        }

    }
}

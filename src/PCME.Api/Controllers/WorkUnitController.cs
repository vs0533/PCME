using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.Commands;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnit")]
    public class WorkUnitController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<WorkUnit> workunitRepository;

        public WorkUnitController(IMediator mediator, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            workunitRepository = unitOfWork.GetRepository<WorkUnit>();
        }

        // GET: api/WorkUnit
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET: api/WorkUnit/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await workunitRepository.FindAsync(c => c.Include(d => d.UnitNature), id);
            if (id <= 0)
            {
                return BadRequest();
            }

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        // POST: api/WorkUnit
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateWorkUnitCommand command)
        {
            bool commandResult = false;
            commandResult = await _mediator.Send(command);
            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();

        }

        // PUT: api/WorkUnit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangePassword(int id, string password)
        {
            if (id < 0)
            {
                return NotFound();
            }
            var workUnitItem = await workunitRepository.FindAsync(id);
            if (workUnitItem == null)
            {
                return NotFound();
            }
            workUnitItem.ChangePassWord(password);
            workunitRepository.Update(workUnitItem);
            await unitOfWork.SaveEntitiesAsync();
            return (IActionResult)Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var workUnitToDelete = await workunitRepository.FindAsync(id);
            if (workUnitToDelete is null)
            {
                return NotFound();
            }

            workunitRepository.Delete(workUnitToDelete);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

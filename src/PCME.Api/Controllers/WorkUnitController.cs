using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public WorkUnitController(IMediator mediator, IUnitOfWork<ApplicationDbContext> unitOfWork, IRepository<WorkUnit> workunitRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.workunitRepository = unitOfWork.GetRepository<WorkUnit>();
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
            var result = await workunitRepository.FindAsync(id);
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
        public async Task<IActionResult> Post([FromBody]CreateWorkUnitCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCancelOrder = new IdentifiedCommand<CreateWorkUnitCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestCancelOrder);
            }

            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();

        }

        // PUT: api/WorkUnit/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var campaignToDelete = await workunitRepository.FindAsync(id);
            if (campaignToDelete is null)
            {
                return NotFound();
            }

            workunitRepository.Delete(id);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

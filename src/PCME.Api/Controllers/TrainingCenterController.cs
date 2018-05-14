using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingCenter")]
    public class TrainingCenterController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<TrainingCenter> trainingCenterRepository;
        public TrainingCenterController(IUnitOfWork<ApplicationDbContext> unitOfWork,IMediator _mediator)
        {
            this.unitOfWork = unitOfWork;
            trainingCenterRepository = unitOfWork.GetRepository<TrainingCenter>();
            this._mediator = _mediator;
        }

		[HttpPost]
        [Route("fetchinfo")]
        public IActionResult FindById(int id)
        {
			var query = trainingCenterRepository.Query(predicate: c => c.Id == id).FirstOrDefault();
            if (id <= 0)
            {
                return BadRequest();
            }

            if (query != null)
            {
                var result = new Dictionary<string, object>{
					{ "id",query.Id},
					{ "name",query.Name},
					{ "logname",query.LogName},
					{ "logpassword",query.LogPassWord},
					{ "address",query.Address}
                };
                return Ok(new { data = result });
            }

            return NotFound();
        }

        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "单位管理员")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var search = trainingCenterRepository.Query(c=>c.Id != 0);

            var item = search.Skip(start).Take(limit);


            var result = item.Select(c => new Dictionary<string, object> {
                { "id",c.Id},
                { "name",c.Name},
                { "logname",c.LogName},
                { "logpassword",c.LogPassWord},
                { "address",c.Address}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "单位管理员,继续教育培训")]
        public async Task<IActionResult> Post([FromBody]TrainingCenterCreateOrUpdateCommand command, string opertype)
        {
            var loginid = int.Parse(User.FindFirstValue("WorkUnitId"));

            var nameExisted = trainingCenterRepository.GetFirstOrDefault(predicate: c =>
                 c.Name == command.Name && c.Id != command.Id
            );
            var logNameExistend = trainingCenterRepository.GetFirstOrDefault(predicate: c =>
                c.LogName == command.LogName && c.Id != command.Id
            );
            if (nameExisted != null)
            {
                ModelState.AddModelError("name", "相同名称的培训点已经存在");
            }
            if (logNameExistend != null)
            {
                ModelState.AddModelError("logname", "相同名称的培训点账号已经存在");
            }
            ModelState.Remove("opertype");
            
            if (ModelState.IsValid)
            {
                TrainingCenter result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
                    { "id", result.Id },
                    { "name",result.Name},
                    { "logname",result.LogName},
                    { "logpassword",result.LogPassWord},
                    { "address",result.Address}
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "单位管理员,继续教育培训")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();
            //var loginid = int.Parse(User.FindFirstValue("id"));
            var delTrainingCenter = await trainingCenterRepository.FindAsync(id);
            if (delTrainingCenter is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            //注意检测是否存在考点
            //var delUnit_IsChild = await trainingCenterRepository.GetFirstOrDefaultAsync(predicate: c => c.PID == delUnit.Id);
            //if (delUnit_IsChild != null)
            //{
            //    return Ok(new { message = "存在下级单位不允许删除" });
            //}


            trainingCenterRepository.Delete(delTrainingCenter);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

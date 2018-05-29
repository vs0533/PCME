using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ExaminationRoom")]
    public class ExaminationRoomController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly IRepository<ExaminationRoom> examinationRoomRepository;
        public ExaminationRoomController(IMediator _mediator, IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext context)
        {
            this._mediator = _mediator;
            this.unitOfWork = unitOfWork;
            this.context = context;
            this.examinationRoomRepository = unitOfWork.GetRepository<ExaminationRoom>();
        }
        [HttpPost]
        [Route("fetchinfo")]
        [Authorize]
        public IActionResult FindById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var search = (from examinationrooms in context.ExaminationRooms
                                   join trainingcenter in context.TrainingCenter on examinationrooms.TrainingCenterId equals trainingcenter.Id into left1
                                   from trainingcenter in left1.DefaultIfEmpty()
                                   where examinationrooms.Id == id
                                   select new { examinationrooms,trainingcenter }).FirstOrDefault();
            
            if (search != null)
            {
                var result = new Dictionary<string, object>{
                    { "id",search.examinationrooms.Id},
                    { "name",search.examinationrooms.Name},
                    { "galleryful",search.examinationrooms.Galleryful},
                    { "description",search.examinationrooms.Description},
                    { "TrainingCenter.Id",search.trainingcenter.Id},
                    { "TrainingCenter.Name",search.trainingcenter.Name}
                };
                return Ok(new { data = result });
            }

            return NotFound();
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "TrainingCenter")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));

            var examinationRooms = from c in context.ExaminationRooms
                                   join t in context.TrainingCenter
                                   on c.TrainingCenterId equals t.Id //into temp
                                   //from tmp in temp.DefaultIfEmpty()
                                   select new { c,t};

            var item = examinationRooms.Skip(start).Take(limit);
            var result = item.ToList().Select(c => new Dictionary<string, object>
                {
                    {"id",c.c.Id},
                    {"name",c.c.Name},
                    {"galleryful",c.c.Galleryful},
                    {"description",c.c.Description},
                    {"TrainingCenter.Id",c.c.TrainingCenterId},
                    {"TrainingCenter.Name",c.t.Name}
                });
            var total = examinationRooms.Count();
            return Ok(new { total, data = result });
        }

        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> Post([FromBody]ExaminationRoomCreateOrUpdateCommand command, string opertype)
        {
            var loginid = int.Parse(User.FindFirstValue("WorkUnitId"));
            if (opertype == "new")
            {
                command.SetId(0);
            }
            command.SetTrainingCenterId(loginid);
            var nameExisted = examinationRoomRepository.GetFirstOrDefault(predicate: c =>
                 c.Name == command.Name && c.Id != command.Id
            );
            if (nameExisted != null)
            {
                ModelState.AddModelError("name", "相同名称的教室已经存在");
            }
            ModelState.Remove("opertype");

            if (ModelState.IsValid)
            {
                ExaminationRoom result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
                    { "id", result.Id },
                    { "name",result.Name},
                    { "galleryful",result.Galleryful},
                    { "description",result.Description}
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

            var del = await examinationRoomRepository.FindAsync(id);
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            #region 注意检测场次

            #endregion

            examinationRoomRepository.Delete(del);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

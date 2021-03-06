﻿using System;
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
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using PCME.Domain.AggregatesModel.SignUpAggregates;
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
        private readonly IRepository<ExaminationRoom> examinationRoomRepository;
        private readonly IRepository<SignUpForUnit> signUpForUnitRepository;
        private readonly ApplicationDbContext context;
        public TrainingCenterController(ApplicationDbContext context,IUnitOfWork<ApplicationDbContext> unitOfWork,IMediator _mediator)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            trainingCenterRepository = unitOfWork.GetRepository<TrainingCenter>();
            this._mediator = _mediator;
            examinationRoomRepository = unitOfWork.GetRepository<ExaminationRoom>();
            signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
        }
        [HttpPost]
        [Route("fetchinfo")]
        [Authorize]
        public IActionResult FindById(int id)
        {
            var query = trainingCenterRepository.Query(predicate: c => c.Id == id)
                .Include(s=>s.OpenType)
                .FirstOrDefault();
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
                    //{ "logpassword",query.LogPassWord},
                    { "address",query.Address},
                    { "OpenType.Id",query.OpenTypeId},
                    { "OpenType.Name",query.OpenType.Name}
                };
                return Ok(new { data = result });
            }

            return NotFound();
        }

        [HttpGet]
        [Route("fetchinfo")]
        [Authorize]
        public IActionResult FindById_Get(int id)
        {
            var query = trainingCenterRepository.Query(predicate: c => c.Id == id)
                .Include(s=>s.OpenType)
                .FirstOrDefault();
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
                    { "address",query.Address},
					{ "OpenType.Id",query.OpenTypeId},
                    { "OpenType.Name",query.OpenType.Name}
                };
                return Ok(new { data = result });
            }

            return NotFound();
        }

		[HttpGet]
        [Route("read")]
		[Authorize(Roles ="Admin")]
        public IActionResult StoreRead()
        {
			var query = trainingCenterRepository.Query(c => c.Id != 0);

            return Ok(query.Select(c => new { value = c.Id, text = c.Name }));
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
                { "address",c.Address},
				{ "OpenType.Id",c.OpenTypeId},
                { "OpenType.Name",c.OpenType.Name},
                {"tel",c.Tel}
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
                    { "address",result.Address},
					{ "OpenType.Id",result.OpenTypeId},
					{ "OpenType.Name",result.OpenType.Name},
                    {"tel",result.Tel}
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
            //注意检测是否存在报名信息
            var signupforunitisExists = signUpForUnitRepository.Query(c => c.TrainingCenterId == delTrainingCenter.Id).Any();
            if (signupforunitisExists)
            {
                return Ok(new { message = "存在 报名信息 不允许删除" });
            }

            var examationroomisExists = examinationRoomRepository.Query(c=>c.TrainingCenterId == delTrainingCenter.Id).Any();
            if (examationroomisExists)
            {
                return Ok(new { message = "存在 教室信息 不允许删除" });
            }


            trainingCenterRepository.Delete(delTrainingCenter);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }

        [HttpPost]
        [Route("changepwd")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> ChangePwd(string oldpwd, string newpwd)
        {
            //var req = JsonConvert.DeserializeObject(str);
            var trainingcenterid = int.Parse(User.FindFirstValue("AccountId"));
            var trainingCenter = await context.TrainingCenter.FindAsync(trainingcenterid); //await context.WorkUnitAccounts.FindAsync(workunitaccountId); //await studentRepository.Query(predicate: c => c.Id == studentId).FirstOrDefaultAsync();
            if (trainingCenter == null)
            {
                return BadRequest(new { success = false, message = "请登录" });
            }
            if (trainingCenter.LogPassWord != oldpwd)
            {
                return BadRequest(new { success = false, message = "旧密码不正确" });
            }
            trainingCenter.ReSetPwd(newpwd);
            await context.SaveChangesAsync();
            return Ok(new { success = true, message = "修改成功" });
        }
    }
}

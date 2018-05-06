using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/WorkUnit")]
    //[Authorize]
    public class WorkUnitController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<WorkUnit> workUnitRepository;
        private readonly ApplicationDbContext _dbContext;

        public WorkUnitController(ApplicationDbContext _dbContext, IMediator mediator, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._dbContext = _dbContext;
            workUnitRepository = unitOfWork.GetRepository<WorkUnit>();
        }

        [HttpPost]
        [Route("fetchinfo")]
        public async Task<IActionResult> FindById(int id)
        {
            var query = await workUnitRepository.FindAsync(c => c.Include(d => d.WorkUnitNature), id);
            if (id <= 0)
            {
                return BadRequest();
            }

            if (query != null)
            {
                //var result = new
                //{
                //    query.Id,
                //    query.Name,
                //    parentname = query.Parent?.Name,
                //    parentid = query.PID,
                //    query.Level,
                //    linkman = query.LinkMan,
                //    linkphone = query.LinkPhone,
                //    workunitnaturename = WorkUnitNature.From(query.WorkUnitNatureId).Name,
                //    workunitnatureid = query.WorkUnitNatureId
                //};
                var result = new Dictionary<string, object>{
                    { "id",query.Id},
                    { "name",query.Name},
                    { "Parent.Name",query.Parent?.Name},
                    { "Parent.Id",query.PID},
                    { "level",query.Level},
                    { "linkman",query.LinkMan},
                    { "linkphone",query.LinkPhone},
                    { "WorkUnitNature.Name",WorkUnitNature.From(query.WorkUnitNatureId).Name},
                    { "WorkUnitNature.Id",query.WorkUnitNatureId}
                };
                return Ok(new { data = result });
            }

            return NotFound();
        }
        [HttpGet]
        [Route("navigatedata")]
        [Authorize(Roles ="Unit")]
        public IActionResult NavigateData(int? id, int? node)
        {
            node = node == 0 ? null : node;
            var _id = node ?? id ?? int.Parse(User.FindFirstValue("WorkUnitId"));
            
            var query = workUnitRepository.Query(c => c.PID == _id, include: s => s.Include(d => d.Childs));
            if (node == null)
            {
                query = workUnitRepository.Query(c => c.Id == _id, include: s => s.Include(d => d.Childs));
            }
            var item = query.Select(d => new
            {
                d.Id,
                text = d.Name,
                d.PID,
                leaf = !d.Childs.Any(),
                fieldvalue = d.Id
            });
            return Ok(item);
        }
        public List<WorkUnit> findallchildren(int PID)
        {
            var list = workUnitRepository.Query(c=>c.PID == PID,include:s=>s.Include(d=>d.Parent)).ToList();

            List<WorkUnit> tmpList = new List<WorkUnit>(list);
            foreach (WorkUnit single in tmpList)
            {
                List<WorkUnit> tmpChildren = findallchildren(single.Id);
                if (tmpChildren.Count != 0)
                {
                    list.AddRange(tmpChildren);
                }
            }
            return list;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "单位管理员")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
            var id = User.FindFirstValue("AccountId");
            var sqlparameId = new SqlParameter("id", navigate == null ? id : navigate.FieldValue.ToString());
            //var search = workUnitRepository.Query(c => c.Id != 0).Include(s=>s.Parent);
            string sql = @"WITH temp  
                            AS  
                            (  
                            SELECT * FROM WorkUnit WHERE id = @id 
                            UNION ALL  
                            SELECT m.* FROM WorkUnit  AS m  
                            INNER JOIN temp AS child ON m.PID = child.Id  
                            )  
                            SELECT * FROM temp";
            var search = workUnitRepository.FromSql(sql, sqlparameId)
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());
            var item = search.Skip(start).Take(limit);

            var result = item.ToList().Select(c => new Dictionary<string, object>
            {
                { "id",c.Id},
                { "name",c.Name},
                { "code",c.Code},
                { "Parent.Name",c.Parent?.Name},
                { "Parent.Id",c.PID},
                { "level",c.Level},
                { "linkman",c.LinkMan},
                { "linkphone",c.LinkPhone},
                { "WorkUnitNature.Name",WorkUnitNature.From(c.WorkUnitNatureId).Name},
                { "WorkUnitNature.Id",c.WorkUnitNatureId}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles = "单位管理员")]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateWorkUnitCommand command,string opertype)
        {
            WorkUnit result = null;
            var loginid = int.Parse(User.FindFirstValue("WorkUnitId"));
            
            var loginobj = await workUnitRepository.FindAsync(loginid);
            if (opertype == "new")
            {
                command.NewInitData(loginid,loginobj.Level);
            }
            
            var nameExisted = workUnitRepository.GetFirstOrDefault(predicate: c =>
                 c.Name == command.Name && c.Id != command.Id
            );
            var codeExistend = workUnitRepository.GetFirstOrDefault(predicate: c =>
                c.Code == command.Code && c.Id != command.Id
            );
            if (nameExisted != null)
            {
                ModelState.AddModelError("name", "单位名称存在");
            }
            if (codeExistend != null)
            {
                ModelState.AddModelError("code", "单位代码存在");
            }
            ModelState.Remove("opertype");
            if (opertype != "new")
            {
                ModelState.Remove("PassWord");
            }
            if (ModelState.IsValid)
            {
                result = await _mediator.Send(command);
                var data = new Dictionary<string, object>
                {
                    { "id", result.Id },
                    { "name",result.Name},
                    { "code",result.Code},
                    { "Parent.Id",result.PID},
                    { "level",result.Level},
                    { "linkman",result.LinkMan},
                    { "linkphone",result.LinkPhone},
                    { "WorkUnitNature.Name",WorkUnitNature.From(result.WorkUnitNatureId).Name},
                    { "WorkUnitNature.Id",result.WorkUnitNatureId}
                };
                return Ok(new { success = true, data });
            }
            return BadRequest();

        }
        [HttpPost]
        [Route("remove")]
        [Authorize(Roles ="Unit")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();
            //var loginid = int.Parse(User.FindFirstValue("id"));
            var delUnit = await workUnitRepository.FindAsync(id);
            if (delUnit is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }
            if (delUnit.Level == 0)
            {
                return Ok(new { message = "系统单位不允许删除" });
            }
            var delUnit_IsChild = await workUnitRepository.GetFirstOrDefaultAsync(predicate: c => c.PID == delUnit.Id);
            if (delUnit_IsChild != null)
            {
                return Ok(new { message = "存在下级单位不允许删除" });
            }


            workUnitRepository.Delete(delUnit);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

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
        private class temp
        {
            public int Id { get; set; }
        }
        [HttpGet]
        [Route("navigatedata")]
        public IActionResult NavigateData(int? id)
        {
            var _id = id ?? int.Parse(User.FindFirstValue("id"));
            var query = workUnitRepository.Query(c => c.PID == _id, include: s => s.Include(d => d.Childs));
            
            var item = query.Select(d => new
            {
                d.Id,
                text = d.Name,
                d.PID,
                leaf = d.Childs.Any()
            });
            return Ok(item);
        }
        [HttpPost]
        [Route("read")]
        public IActionResult StoreRead(int start, int limit, string filter)
        {
            var id = User.FindFirstValue("id");
            var sqlparameId = new SqlParameter("id", id);

            string sql = @"WITH temp  
                            AS  
                            (  
                            SELECT * FROM Unit WHERE id = @id 
                            UNION ALL  
                            SELECT m.* FROM Unit  AS m  
                            INNER JOIN temp AS child ON m.PID = child.Id  
                            )  
                            SELECT * FROM temp";

            var query = workUnitRepository.FromSql(sql, sqlparameId).Filter(filter.ToFilter());
            var item = query.Skip(start).Take(limit);

            temp t = new temp();

            var result = item.Select(c => new
            {
                c.Id,
                c.Name,
                parentname = c.Parent.Name,
                c.Level,
                linkman = c.LinkMan,
                linkphone = c.LinkPhone,
                workunitnaturename = WorkUnitNature.From(c.WorkUnitNatureId).Name,
                workunitnatureid = c.WorkUnitNatureId
            });
            var total = query.Count();
            return Ok(new { total, data = result });
        }
        [HttpPost]
        //[Authorize(Roles = "Unit")]
        [Route("getpagedlist")]
        public IActionResult GetPagedList(string unitname, int id, int start, int limit, string filter)
        {
            var sqlparameId = new SqlParameter("id", id);
            string sql = @"WITH temp  
                            AS  
                            (  
                            SELECT * FROM Unit WHERE id = @id 
                            UNION ALL  
                            SELECT m.* FROM Unit  AS m  
                            INNER JOIN temp AS child ON m.PID = child.Id  
                            )  
                            SELECT * FROM temp";

            var query = workUnitRepository.FromSql(sql, sqlparameId).Filter(filter.ToFilter());

            if (!string.IsNullOrEmpty(unitname))
            {
                query = query.Where(c => c.Name.Contains(unitname));
            }

            var item = query.Skip(start).Take(limit);
            var result = item.Select(c => new
            {
                c.Id,
                c.Name,
                parentname = c.Parent.Name,
                c.Level,
                linkman = c.LinkMan,
                linkphone = c.LinkPhone,
                workunitnature = WorkUnitNature.From(c.WorkUnitNatureId).Name
            });
            var total = query.Count();
            return Ok(new { total, data = result });
        }
        

        [HttpGet]
        [Route("child")]
        [Authorize(Roles = "Unit")]
        public IActionResult GetUnitTreeByParentId(int id, string unitname)
        {
            //var items = await workunitRepository.GetPagedListAsync(
            //   predicate: c=>c.PID ==pid,pageIndex:0,pageSize:int.MaxValue
            //    );

            var query = workUnitRepository.Query(c => c.PID == id, include: s => s.Include(d => d.Childs));
            if (!string.IsNullOrEmpty(unitname))
            {
                var sqlparameId = new SqlParameter("id", id);
                var sqlparameName = new SqlParameter("name", string.Format("%{0}%", unitname));

                string sql = @"WITH temp  
                            AS  
                            (  
                            SELECT * FROM Unit WHERE id = @id 
                            UNION ALL  
                            SELECT m.* FROM Unit  AS m  
                            INNER JOIN temp AS child ON m.PID = child.Id  
                            )  
                            SELECT * FROM temp where temp.name like @name";
                query = workUnitRepository.FromSql(sql, sqlparameId, sqlparameName);
            }
            var item = query.Select(d => new
            {
                id = d.Id,
                text = d.Name,
                parentId = d.PID,
                leaf = !d.Childs.Any()
            });
            return Ok(item);
        }

        // GET: api/WorkUnit/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await workUnitRepository.FindAsync(c => c.Include(d => d.UnitNature), id);
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
        [Route("saveorupdate")]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateWorkUnitCommand command)
        {
            //var command = new CreateOrUpdateWorkUnitCommand(command.);
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
            var workUnitItem = await workUnitRepository.FindAsync(id);
            if (workUnitItem == null)
            {
                return NotFound();
            }
            workUnitItem.ChangePassWord(password);
            workUnitRepository.Update(workUnitItem);
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

            var workUnitToDelete = await workUnitRepository.FindAsync(id);
            if (workUnitToDelete is null)
            {
                return NotFound();
            }

            workUnitRepository.Delete(workUnitToDelete);
            await unitOfWork.SaveEntitiesAsync();
            return NoContent();
        }
    }
}

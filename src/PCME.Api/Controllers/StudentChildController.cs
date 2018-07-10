using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
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
    [Route("api/StudentChild")]
    public class StudentChildController:Controller
    {
        
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<WorkUnit> workUnitRepository;
        public StudentChildController(IUnitOfWork<ApplicationDbContext> unitOfWork, IMediator _mediator)
        {
            this.unitOfWork = unitOfWork;
            studentRepository = unitOfWork.GetRepository<Student>();
            workUnitRepository = unitOfWork.GetRepository<WorkUnit>();
            this._mediator = _mediator;
        }
        [HttpGet]
        [Route("navigatedata")]
        [Authorize(Roles = "Unit")]
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
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "单位管理员,继续教育培训")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            var loginUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));

            var navigate = navigates.ToObject<Navigate>().FirstOrDefault();
            int searchid = navigate == null ? loginUnitId : navigate.FieldValue;

            var search = studentRepository.Query(f => f.WorkUnitId == searchid,
                include: s => s
                 .Include(c => c.StudentType)
                 .Include(c => c.Sex)
                 .Include(c => c.StudentStatus))
                 .Include(c => c.WorkUnit)
                 .FilterAnd(filter.ToObject<Filter>())
                 .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);


            var result = item.Select(c => new Dictionary<string, object> {
                { "id",c.Id},
                { "name",c.Name},
                { "idcard",c.IDCard},
                { "officename",c.OfficeName},
                { "password",c.Password},
                { "Sex.Id",c.SexId},
                { "Sex.Name",c.Sex.Name},
                { "StudentType.Id",c.StudentTypeId},
                { "StudentType.Name",c.StudentType.Name},
                { "StudentStatus.Id",c.StudentStatusId},
                { "StudentStatus.Name",c.StudentStatus.Name},
                { "graduationschool",c.GraduationSchool},
                { "specialty",c.Specialty},
                { "workdate",c.WorkDate},
                { "WorkUnit.Id",c.WorkUnitId},
                { "WorkUnit.Name",c.WorkUnit.Name},
                { "address",c.Address},
                { "email",c.Email},
                { "birthday",c.BirthDay}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
    }
}

using MediatR;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ChangeStudentUnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PCME.Api.Application.Commands
{
    public class ChangeStudentUnitCreateOrUpdateHandler : IRequestHandler<ChangeStudentUnitCreateOrUpdateCommand, Dictionary<string, object>>
    {
        private readonly ApplicationDbContext context;
        public ChangeStudentUnitCreateOrUpdateHandler(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Dictionary<string, object>> Handle(ChangeStudentUnitCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isExists = await context.ChangeStudentUnit.FindAsync(request.Id);
            
            if (isExists == null)
            {
                ChangeStudentUnit changeStudentUnit = new ChangeStudentUnit(
                    request.OldUnitId
                    , request.NewUnitId
                    , request.StudentId
                    , request.AuditStatusId
                    ,DateTime.Now
                    );
                context.ChangeStudentUnit.Add(changeStudentUnit);
                await context.SaveChangesAsync();
                return Return(changeStudentUnit.Id);
            }
            else
            {
                //isExists.Update(request.OldUnitId, request.NewUnitId, request.StudentId, request.AuditStatusId, DateTime.Now);
                
                if (isExists.AuditStatusId == AuditStatus.Wait.Id && request.AuditStatusId == AuditStatus.Pass.Id)
                {
                    isExists.Audit(request.AuditStatusId, DateTime.Now);
                    var student = await context.Students.FirstOrDefaultAsync(c => c.Id == isExists.StudentId);
                    student.UpdateWorkUnit(isExists.NewUnitId);
                    context.Students.Update(student);
                }
                context.ChangeStudentUnit.Update(isExists);
                await context.SaveChangesAsync();
                return Return(isExists.Id);
            }
        }
        public Dictionary<string, object> Return(int key)
        {
            var search = from changestudentunit in context.ChangeStudentUnit
                         join student in context.Students on changestudentunit.StudentId equals student.Id
                         join oldunit in context.WorkUnits on changestudentunit.OldUnitId equals oldunit.Id
                         join newunit in context.WorkUnits on changestudentunit.NewUnitId equals newunit.Id
                         join auditstatus in context.AuditStatus on changestudentunit.AuditStatusId equals auditstatus.Id
                         where changestudentunit.Id == key
                         orderby changestudentunit.CreateTime descending, changestudentunit.AuditStatusId ascending
                         select new { changestudentunit, student, oldunit, newunit,auditstatus };

            var result = search.Select(c => new Dictionary<string, object>
            {
                {"id",c.changestudentunit.Id},
                {"student.IDCard",c.student.IDCard},
                {"student.Id",c.student.Id},
                {"student.Name",c.student.Name},
                {"oldunit.Id",c.oldunit.Id},
                {"oldunit.Name",c.oldunit.Name},
                {"newunit.Id",c.newunit.Id},
                {"newunit.Name",c.newunit.Name},
                {"changestudentunit.CreateTime",c.changestudentunit.CreateTime},
                {"changestudentunit.AuditStatusTime",c.changestudentunit.AuditStatusTime},
                {"auditstatus.Id",c.auditstatus.Id},
                {"auditstatus.Name",c.auditstatus.Name}
            }).FirstOrDefault();
            return result;
        }
    }
}

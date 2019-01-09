using System;
using System.Collections.Generic;
using System.Threading;
using MediatR;
using System.Threading.Tasks;
using PCME.Infrastructure;
using PCME.Domain.AggregatesModel.ApplicationForm;

namespace PCME.Api.Application.Commands
{
    public class ApplyTableCreateOrUpdateCommandHandler: IRequestHandler<ApplyTableCreateOrUpdateCommand, Dictionary<string, object>>
    {
        private readonly ApplicationDbContext context;
        public ApplyTableCreateOrUpdateCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Dictionary<string, object>> Handle(ApplyTableCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            //var isExists = await context.ApplyTable.FindAsync(request.Id);

            //if (isExists == null)
            //{
            //    ApplyTable applyTable = new ApplyTable(
            //        request.WorkUnitId,
            //        request.CreateTime,
            //        request.Num
            //        );
            //    context.ApplyTable.Add(applyTable);
            //    await context.SaveChangesAsync();
            //    return Return(changeStudentUnit.Id);
            //}
            //else
            //{ 

            //}
            throw new Exception("error");
        }
    }
}

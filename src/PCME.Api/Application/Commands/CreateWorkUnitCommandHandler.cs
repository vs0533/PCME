using AutoMapper;
using MediatR;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class CreateWorkUnitCommandHandler: IRequestHandler<CreateWorkUnitCommand, bool>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<WorkUnit> workUnitRepository;

        public CreateWorkUnitCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.workUnitRepository = unitOfWork.GetRepository<WorkUnit>() ?? throw new ArgumentNullException(nameof(workUnitRepository));
        }
        public async Task<bool> Handle(CreateWorkUnitCommand request, CancellationToken cancellationToken)
        {
            var workUnitToUpdate = await workUnitRepository.FindAsync(request.Id);
            var workUnit = Mapper.Map<CreateWorkUnitCommand, WorkUnit>(request);
            if (workUnitToUpdate == null)
            {
                await workUnitRepository.InsertAsync(workUnit);
            }
            else
            {
                workUnitRepository.Update(workUnit);
            }

            return await unitOfWork.SaveEntitiesAsync();
        }
    }
}

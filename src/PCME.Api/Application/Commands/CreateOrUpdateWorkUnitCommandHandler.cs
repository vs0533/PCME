using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class CreateOrUpdateWorkUnitCommandHandler : IRequestHandler<CreateOrUpdateWorkUnitCommand, WorkUnit>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<WorkUnit> workUnitRepository;

        public CreateOrUpdateWorkUnitCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.workUnitRepository = unitOfWork.GetRepository<WorkUnit>() ?? throw new ArgumentNullException(nameof(workUnitRepository));
        }
        public async Task<WorkUnit> Handle(CreateOrUpdateWorkUnitCommand request, CancellationToken cancellationToken)
        {
            var idIsExisted = await workUnitRepository.FindAsync(request.Id);

            //var workUnit1 = Mapper.Map<CreateOrUpdateWorkUnitCommand, WorkUnit>(request);


            if (idIsExisted == null)//新增单位
            {
                var workUnit = new WorkUnit(
                       request.Code,
                       request.Name,
                       request.Level,
                       request.LinkMan,
                       request.LinkPhone,
                       request.Email,
                       request.Address,
                       request.PID,
                       request.WorkUnitNatureId,
                       WorkUnitAccountType.Manager.Id, null, request.PassWord
                   );
                try
                {
                    await workUnitRepository.InsertAsync(workUnit);
                    await unitOfWork.SaveEntitiesAsync();
                    return workUnit;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else//更新单位
            {
                idIsExisted.Update(request.Name, request.LinkMan, request.Email, request.LinkPhone, request.Address,request.WorkUnitNatureId);
                workUnitRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return idIsExisted;
            }
        }
    }
}

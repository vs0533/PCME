using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class CreateOrUpdateWorkUnitCommandHandler : IRequestHandler<CreateOrUpdateWorkUnitCommand, bool>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<WorkUnit> workUnitRepository;

        public CreateOrUpdateWorkUnitCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.workUnitRepository = unitOfWork.GetRepository<WorkUnit>() ?? throw new ArgumentNullException(nameof(workUnitRepository));
        }
        public async Task<bool> Handle(CreateOrUpdateWorkUnitCommand request, CancellationToken cancellationToken)
        {
            var idIsExisted = await workUnitRepository.FindAsync(request.Id);

            var codeIsExisted = await workUnitRepository.GetFirstOrDefaultAsync(s=>new { code = s.Code }, c => c.Code == request.Code && c.Id != request.Id);
            var nameIsExisted = await workUnitRepository.GetFirstOrDefaultAsync(s=>new { name = s.Name},c => c.Name == request.Name && c.Id != request.Id);

            //单位名称存在
            if (nameIsExisted != null) { return false; }
            //单位代码存在
            if (codeIsExisted != null) { return false; }

            var workUnit = Mapper.Map<CreateOrUpdateWorkUnitCommand, WorkUnit>(request);

            if (idIsExisted == null)//新增单位
            {
                await workUnitRepository.InsertAsync(workUnit);
            }
            else//更新单位
            {
                idIsExisted.Update(request.Name, request.LinkMan, request.Email, request.LinkPhone, request.Address);
                workUnitRepository.Update(idIsExisted);

            }
            return await unitOfWork.SaveEntitiesAsync();
        }
    }
}

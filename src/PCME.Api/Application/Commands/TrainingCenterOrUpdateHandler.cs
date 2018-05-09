﻿using MediatR;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class TrainingCenterOrUpdateHandler : IRequestHandler<TrainingCenterCreateOrUpdateCommand, TrainingCenter>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<TrainingCenter> trainingCenterRepository;
        public TrainingCenterOrUpdateHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            trainingCenterRepository = this.unitOfWork.GetRepository<TrainingCenter>();
        }
        public async Task<TrainingCenter> Handle(TrainingCenterCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var idIsExisted = await trainingCenterRepository.FindAsync(request.Id);
            if (idIsExisted == null) //新增
            {
                TrainingCenter tc = new TrainingCenter(
                    request.LogName
                    ,request.LogPassWord
                    ,request.Name
                    ,request.Address
                    );
                await trainingCenterRepository.InsertAsync(tc);
                await unitOfWork.SaveEntitiesAsync();
                return tc;
            }
            else
            {
                idIsExisted.Update(
                       request.LogName
                    , request.LogPassWord
                    , request.Name
                    , request.Address
                    );
                trainingCenterRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return idIsExisted;
            }
        }
    }
}

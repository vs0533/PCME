using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomCreateOrUpdateCommandHandler : IRequestHandler<ExaminationRoomCreateOrUpdateCommand, ExaminationRoom>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<ExaminationRoom> examinationRepository;
        public ExaminationRoomCreateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            examinationRepository = this.unitOfWork.GetRepository<ExaminationRoom>();
        }
        public async Task<ExaminationRoom> Handle(ExaminationRoomCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            ExaminationRoom returnInfo = null;
            var isExists = await examinationRepository.FindAsync(request.Id);
            if (isExists != null)
            {
                isExists.Update(request.Name, 0, request.Description);
                examinationRepository.Update(isExists);
                returnInfo = isExists;
            }
            else {
                ExaminationRoom examinationRoom = new ExaminationRoom(request.Num, request.Name,0,request.TrainingCenterId,request.Description);
                await examinationRepository.InsertAsync(examinationRoom);
                returnInfo = examinationRoom;
            }
            await unitOfWork.SaveChangesAsync();
            return returnInfo;
        }
    }
}

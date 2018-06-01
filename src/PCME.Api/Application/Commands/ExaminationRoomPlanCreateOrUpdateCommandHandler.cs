using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomPlanCreateOrUpdateCommandHandler : IRequestHandler<ExaminationRoomPlanCreateOrUpdateCommand, ExaminationRoomPlan>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<ExaminationRoomPlan> examinationRoomPlanRepository;
        public ExaminationRoomPlanCreateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            examinationRoomPlanRepository = unitOfWork.GetRepository<ExaminationRoomPlan>();
        }
        public async Task<ExaminationRoomPlan> Handle(ExaminationRoomPlanCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isexists = await examinationRoomPlanRepository.FindAsync(request.Id);
            if (isexists != null)
            {
                isexists.Update(
                         request.ExaminationRoomId
                       , request.Num
                       , request.SelectTime
                       , request.SelectFinishTime
                       , request.SignInTime
                       , request.ExamEndTime
                       , request.ExamStartTime
                       , request.ExamSubjectID
                       , request.AuditStatusId
                       , request.PlanStatusId
                    );

                examinationRoomPlanRepository.Update(isexists);
                await unitOfWork.SaveChangesAsync();
                return isexists;
            }
            else
            {
                ExaminationRoomPlan plan = new ExaminationRoomPlan(
                        request.ExaminationRoomId
                        ,request.Num
                        ,request.SelectTime
                        ,request.SelectFinishTime
                        ,request.SignInTime
                        ,request.ExamEndTime
                        ,request.ExamStartTime
                        ,request.ExamSubjectID
                        ,request.AuditStatusId
                        ,request.PlanStatusId
                        ,request.TrainingCenterId
                    );

                await examinationRoomPlanRepository.InsertAsync(plan);
                await unitOfWork.SaveChangesAsync();
                return plan;
            }
        }
    }
}

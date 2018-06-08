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
    public class ExaminationRoomPlanCreateOrUpdateCommandHandler : IRequestHandler<ExaminationRoomPlanCreateOrUpdateCommand, Dictionary<string,object>>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<ExaminationRoomPlan> examinationRoomPlanRepository;
        private readonly ApplicationDbContext context;
        public ExaminationRoomPlanCreateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork,ApplicationDbContext context)
        {
            this.unitOfWork = unitOfWork;
            examinationRoomPlanRepository = unitOfWork.GetRepository<ExaminationRoomPlan>();
            this.context = context;
        }
        public async Task<Dictionary<string, object>> Handle(ExaminationRoomPlanCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isexists = await examinationRoomPlanRepository.FindAsync(request.Id);
            if (isexists != null)
            {
                isexists.Update(
                         request.ExaminationRoomId
                       , request.Galleryful
                       , request.SelectTime
                       , request.SelectFinishTime
                       , request.SignInTime
                       , request.ExamEndTime
                       , request.ExamStartTime
                       , request.AuditStatusId
                       , request.PlanStatusId
                    );

                examinationRoomPlanRepository.Update(isexists);
                await unitOfWork.SaveChangesAsync();
                return GetShow(isexists);
            }
            else
            {
                ExaminationRoomPlan plan = new ExaminationRoomPlan(
                        request.ExaminationRoomId
                        ,request.Num
                        ,request.Galleryful
                        ,request.SelectTime
                        ,request.SelectFinishTime
                        ,request.SignInTime
                        ,request.ExamEndTime
                        ,request.ExamStartTime
                        ,request.AuditStatusId
                        ,request.PlanStatusId
                        ,request.TrainingCenterId
                    );

                await examinationRoomPlanRepository.InsertAsync(plan);
                await unitOfWork.SaveChangesAsync();
                return GetShow(plan);
            }
        }
        private Dictionary<string,object> GetShow(ExaminationRoomPlan roomPlan)
        {
            var examinationRoomPlan = from examinationroomplans in context.ExaminationRoomPlans
                                      join examinationrooms in context.ExaminationRooms on examinationroomplans.ExaminationRoomId equals examinationrooms.Id into left2
                                      from examinationrooms in left2.DefaultIfEmpty()
                                      join auditstatus in context.AuditStatus on examinationroomplans.AuditStatusId equals auditstatus.Id into left3
                                      from auditstatus in left3.DefaultIfEmpty()
                                      join planstatus in context.PlanStatus on examinationroomplans.PlanStatusId equals planstatus.Id into left4
                                      from planstatus in left4.DefaultIfEmpty()
                                      where examinationroomplans.Id == roomPlan.Id
                                      select new { examinationroomplans, examinationrooms, auditstatus, planstatus };
            var item = examinationRoomPlan.FirstOrDefault();
            var result = new Dictionary<string, object> {
                { "id",item.examinationroomplans.Id},
                { "examinationroomplans.Num",item.examinationroomplans.Num},
                { "examinationroomplans.Galleryful",item.examinationroomplans.Galleryful},
                { "examinationroomplans.SelectTime",item.examinationroomplans.SelectTime},
                { "examinationroomplans.SelectFinishTime",item.examinationroomplans.SelectFinishTime},
                { "examinationroomplans.SignInTime",item.examinationroomplans.SignInTime},
                { "examinationroomplans.ExamEndTime",item.examinationroomplans.ExamEndTime},
                { "examinationroomplans.ExamStartTime",item.examinationroomplans.ExamStartTime},
                { "examinationrooms.Id",item.examinationrooms.Id},
                { "examinationrooms.Name",item.examinationrooms.Name},
                { "auditstatus.Id",item.auditstatus.Id},
                { "auditstatus.Name",item.auditstatus.Name},
                { "planstatus.Id",item.planstatus.Id},
                { "planstatus.Name",item.planstatus.Name}
            };
            return result;
        }
    }
}

using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomAccountAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomAccountCommandHandler : IRequestHandler<ExaminationRoomAccountCreateOrUpdateCommand, Dictionary<string, object>>
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;
        public ExaminationRoomAccountCommandHandler(IMediator mediator, ApplicationDbContext context)
        {
            this.context = context;
            this.mediator = mediator;
        }
        public async Task<Dictionary<string, object>> Handle(ExaminationRoomAccountCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isExists = await context.ExaminationRoomAccount.FindAsync(request.Id);
            if (isExists == null)
            {
                ExaminationRoomAccount examinationRoomAccount = new ExaminationRoomAccount(request.AccountName
                    , request.Password, request.TrainingCenterId, request.ExaminationRoomId, DateTime.Now);
                context.ExaminationRoomAccount.Add(examinationRoomAccount);
                await context.SaveChangesAsync();
                return Return(examinationRoomAccount.Id);
            }
            else
            {
                isExists.Update(request.AccountName
                    , request.Password, request.TrainingCenterId, request.ExaminationRoomId);
                context.ExaminationRoomAccount.Update(isExists);
                await context.SaveChangesAsync();
                return Return(isExists.Id);
            }
        }

        private Dictionary<string, object> Return(int key) {
            var query = from examinationroomaccount in context.ExaminationRoomAccount
                        join trainingcenter in context.TrainingCenter on examinationroomaccount.TrainingCenterId equals trainingcenter.Id
                        join examinationroom in context.ExaminationRooms on examinationroomaccount.ExaminationRoomId equals examinationroom.Id
                        where examinationroomaccount.Id == key
                        select new { examinationroomaccount, trainingcenter, examinationroom };
            var result = query.Select(c => new Dictionary<string, object>
            {
                {"examinationroomaccount.Id",c.examinationroomaccount.Id},
                {"examinationroomaccount.AccountName",c.examinationroomaccount.AccountName},
                {"examinationroomaccount.Password",c.examinationroomaccount.Password},
                {"trainingcenter",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"examinationroom.Id",c.examinationroom.Id},
                {"examinationroom.Name",c.examinationroom.Name}
            }).FirstOrDefault();
            return result;
        }
    }
}

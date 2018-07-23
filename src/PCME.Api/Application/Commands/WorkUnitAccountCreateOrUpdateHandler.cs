using MediatR;
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
    public class WorkUnitAccountCreateOrUpdateHandler : IRequestHandler<WorkUnitAccountCreateOrUpdateCommand, Dictionary<string, object>>
    {
        private readonly ApplicationDbContext context;
        private readonly IRepository<WorkUnitAccount> workUnitAccountRepository;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        public WorkUnitAccountCreateOrUpdateHandler(ApplicationDbContext context, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            workUnitAccountRepository = this.unitOfWork.GetRepository<WorkUnitAccount>();
        }

        public async Task<Dictionary<string, object>> Handle(WorkUnitAccountCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isExists = await workUnitAccountRepository.FindAsync(request.Id);
            if (isExists != null)
            {
                isExists.Update(request.PassWord,request.HolderName);
                workUnitAccountRepository.Update(isExists);
                await unitOfWork.SaveChangesAsync();
                return GetShow(isExists.Id);

            }
            else
            {
                WorkUnitAccount account = new WorkUnitAccount(request.AccountName, request.WorkUnitAccountTypeId
                    ,request.PassWord,request.HolderName);
                account.SetWorkUnitId(request.WorkUnitId ?? 0);
                await workUnitAccountRepository.InsertAsync(account);
                await unitOfWork.SaveChangesAsync();
                return GetShow(account.Id);
            }
        }
        public Dictionary<string, object> GetShow(int key)
        {
            var search = from workunitaccount in context.WorkUnitAccounts
                         join workunit in context.WorkUnits on workunitaccount.WorkUnitId equals workunit.Id into left1
                         from workunit in left1.DefaultIfEmpty()
                         join workunitaccounttype in context.WorkUnitAccountType on workunitaccount.WorkUnitAccountTypeId equals workunitaccounttype.Id into left2
                         from workunitaccounttype in left2.DefaultIfEmpty()
                         where workunitaccount.Id == key
                         select new { workunitaccount, workunit, workunitaccounttype };
            var result = search.Select(c => new Dictionary<string, object>
            {
                { "id",c.workunitaccount.Id},
                { "workunitaccount.AccountName",c.workunitaccount.AccountName},
                { "workunitaccount.PassWord",c.workunitaccount.PassWord},
                { "workunit.Id",c.workunit.Id},
                { "workunit.Name",c.workunit.Name},
                { "workunitaccount.HolderName",c.workunitaccount.HolderName},
                { "workunitaccounttype.Id",c.workunitaccounttype.Id},
                { "workunitaccounttype.Name",c.workunitaccounttype.Name}
            }).FirstOrDefault();
            return result;
        }
    }
}

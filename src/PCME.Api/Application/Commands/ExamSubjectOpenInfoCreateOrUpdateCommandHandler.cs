using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Application.Commands
{
	public class ExamSubjectOpenInfoCreateOrUpdateCommandHandler: IRequestHandler<ExamSubjectOpenInfoCreateOrUpdateCommand, ExamSubjectOpenInfo>
    {
		private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<ExamSubjectOpenInfo> examSubjectOpenInfoRepository;

		public ExamSubjectOpenInfoCreateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
			this.unitOfWork = unitOfWork;
			examSubjectOpenInfoRepository = unitOfWork.GetRepository<ExamSubjectOpenInfo>();
        }

		public async Task<ExamSubjectOpenInfo> Handle(ExamSubjectOpenInfoCreateOrUpdateCommand request, CancellationToken cancellationToken)
		{
			var idIsExisted = await examSubjectOpenInfoRepository.FindAsync(request.Id);


            if (idIsExisted == null)//新增单位
            {
                var examSubjectOpenInfo = new ExamSubjectOpenInfo(
					request.TrainingCenterId ?? 0
					,request.ExamSubjectId
					,request.SignUpTime
					,request.SignUpFinishTime
					,request.SignUpFinishOffset
                    ,request.DisplayExamTime
					,request.AuditStatusId ?? 0
                   );
                try
                {
					await examSubjectOpenInfoRepository.InsertAsync(examSubjectOpenInfo);
                    await unitOfWork.SaveEntitiesAsync();

					return await GetShow(examSubjectOpenInfo);
                }
                catch (Exception ex)
                {               
                    throw;
                }
            }
            else//更新单位
            {
				idIsExisted.Update(request.TrainingCenterId ?? 0
					               ,request.ExamSubjectId
				                   ,request.SignUpTime
				                   ,request.SignUpFinishTime
				                   ,request.SignUpFinishOffset
				                   ,request.DisplayExamTime
				                   ,request.AuditStatusId ?? 0);
				examSubjectOpenInfoRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return await GetShow(idIsExisted);
            }         
		}

		private async Task<ExamSubjectOpenInfo> GetShow(ExamSubjectOpenInfo examSubjectOpenInfo)
        {
            var result = await examSubjectOpenInfoRepository.Query(c => c.Id == examSubjectOpenInfo.Id)
                                                            .Include(s => s.AuditStatus)
                                                            .Include(s => s.ExamSubject)
                                                            .Include(s => s.TrainingCenter).FirstOrDefaultAsync();
            return result;
        }
	}
}
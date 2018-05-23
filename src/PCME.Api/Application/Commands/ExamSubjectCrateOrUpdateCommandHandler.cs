using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;

namespace PCME.Api.Application.Commands
{
	public class ExamSubjectCrateOrUpdateCommandHandler:IRequestHandler<ExamSubjectCreateOrUpdateCommand,ExamSubject>
    {
		
		private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
		private readonly IRepository<ExamSubject> examSubjectRepository;

		public ExamSubjectCrateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
			this.unitOfWork = unitOfWork;
			examSubjectRepository = this.unitOfWork.GetRepository<ExamSubject>();
        }

		public async Task<ExamSubject> Handle(ExamSubjectCreateOrUpdateCommand request, CancellationToken cancellationToken)
		{
			var idIsExisted = await examSubjectRepository.FindAsync(request.Id);
         

            if (idIsExisted == null)//新增单位
            {
                var examSubject = new ExamSubject(
                       request.Code
                    ,request.Name
					,request.OpenTypeId
					,request.ExamTypeId
					,request.SeriesId
					,request.ExamSubjectStatusId
                    ,request.MSCount
                    ,request.CreditHour
                   );
                try
                {
					await examSubjectRepository.InsertAsync(examSubject);
                    await unitOfWork.SaveEntitiesAsync();

                    return await GetShow(examSubject);
                }
                catch
                {
                    throw;
                }
            }
            else//更新单位
            {
				idIsExisted.Update(request.Code
                    , request.Name
                    , request.OpenTypeId
                    , request.ExamTypeId
                    , request.SeriesId
                    , request.ExamSubjectStatusId
                    , request.MSCount
                    , request.CreditHour);
				examSubjectRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return await GetShow(idIsExisted);
            }
		}
		private async Task<ExamSubject> GetShow(ExamSubject examSubject){
			var result = await examSubjectRepository.Query(c => c.Id == examSubject.Id)
											  .Include(s => s.ExamSubjectStatus)
											  .Include(s => s.ExamType)
											  .Include(s => s.OpenType)
			                                  .Include(s => s.Series).FirstOrDefaultAsync();
			return result;
		}
	}
}

using MediatR;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class StudentCreateOrUpdateCommandHandler : IRequestHandler<StudentCreateOrUpdateCommand,Student>
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<Student> studentRepository;
        public StudentCreateOrUpdateCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.studentRepository = unitOfWork.GetRepository<Student>();
        }

        public async Task<Student> Handle(StudentCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var idIsExisted = await studentRepository.FindAsync(request.Id);
            if (idIsExisted == null) //新增
            {
                Student sd = new Student(
                        request.Name
                        , request.IDCard
                        , request.Password
                        , request.SexId
                        , request.StudentTypeId
                        , request.BirthDay
                        , request.GraduationSchool
                        , request.Specialty
                        , request.WorkDate
                        , request.OfficeName
                        , null, false
                        , request.Email, false
                        , request.Address
                        , 0, 0
                        , request.WorkUnitId
                        , request.StudentStatusId
                    );
                await studentRepository.InsertAsync(sd);
                await unitOfWork.SaveEntitiesAsync();
                return sd;
            }
            else
            {
                idIsExisted.Update(
                        request.Name
                        , request.Password
                        , request.SexId
                        , request.StudentTypeId
                        , request.BirthDay
                        , request.GraduationSchool
                        , request.Specialty
                        , request.WorkDate
                        , request.OfficeName
                        , request.Address
                        , request.StudentStatusId
                    );
                studentRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return idIsExisted;
            }
        }
    }
}

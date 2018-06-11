using MediatR;
using Microsoft.AspNetCore.Hosting;
using PCME.Api.Infrastructure;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class StudentCreateOrUpdateCommandHandler : IRequestHandler<StudentCreateOrUpdateCommand,Student>
    {
        private readonly IHostingEnvironment hostingEnv;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<Student> studentRepository;
        public StudentCreateOrUpdateCommandHandler(IHostingEnvironment hostingEnv,IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            studentRepository = unitOfWork.GetRepository<Student>();
            this.hostingEnv = hostingEnv;
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
                        , request.Email
                    );
                if ((!string.IsNullOrEmpty(request.Favicon)) && !idIsExisted.EmailIsValid)
                {
                    string filename = idIsExisted.Id.ToString() + ".jpg";
                    string filepath = Path.Combine(hostingEnv.WebRootPath, "Files", filename);
                    ImageHelper.Base64StringToImage(request.Favicon, filepath);
                    idIsExisted.UpdatePhoto(filename, true);
                }
                studentRepository.Update(idIsExisted);
                await unitOfWork.SaveEntitiesAsync();
                return idIsExisted;
            }
        }
    }
}

using MediatR;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.SignUpStudentAggregates;

namespace PCME.Api.Application.Commands
{
    public class SignUpStudentCreateOrUpdateCommandHandler : IRequestHandler<SignUpStudentCreateOrUpdateCommand, Dictionary<string, object>>
    {
        private readonly ApplicationDbContext context;
        public SignUpStudentCreateOrUpdateCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Dictionary<string, object>> Handle(SignUpStudentCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isExists = await context.SignUpStudent.Include(s=>s.Collection).Where(c => c.StudentId == request.StudentId && c.Id == request.Id).FirstOrDefaultAsync();
            if (isExists != null)
            {
                List<SignUpStudentCollection> signUpStudentCollections = new List<SignUpStudentCollection>();
                foreach (var item in request.CollectionDTO)
                {
                    signUpStudentCollections.Add(new SignUpStudentCollection(item.ExamSubjectId));
                }
                isExists.Update(request.Code, request.StudentId, request.TrainingCenterId,signUpStudentCollections);
                context.SignUpStudent.Update(isExists);
                await context.SaveChangesAsync();
                return Return(isExists.Id);
            }
            else
            {
                SignUpStudent signup = new SignUpStudent(
                    request.Code
                    ,request.StudentId
                    ,request.TrainingCenterId,
                    DateTime.Now
                    );
                foreach (var item in request.CollectionDTO)
                {
                    signup.AddCollection(item.ExamSubjectId);
                }
                context.SignUpStudent.Add(signup);
                await context.SaveChangesAsync();
                return Return(signup.Id);
            }
        }

        public  Dictionary<string, object> Return(int key)
        {
            var query = from signupstudent in context.SignUpStudent.Include(s=>s.Collection)
                        join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                        join student in context.Students on signupstudent.StudentId equals student.Id
                        where signupstudent.Id == key
                        select new { signupstudent, trainingcenter, student };
            var result = query.Select(c => new Dictionary<string, object>
            {
                {"id",c.trainingcenter.Id},
                {"Collection",c.signupstudent.Collection.Join(
                    context.ExamSubjects,
                    l=>l.ExamSubjectId,
                    r=>r.Id,
                    (l,r)=>new { l,r}
                    ).Select(s=>new Dictionary<string,object>{
                    {"examsubjectid",s.l.ExamSubjectId},
                    {"examsubjectname",s.r.Name}
                })},
                {"trainingcenter.Id",c.trainingcenter.Id},
                {"trainingcenter.Name",c.trainingcenter.Name},
                {"student.Id",c.student.Id},
                {"student.Name",c.student.Name},
                {"student.IDCard",c.student.IDCard}
            });
            return result.FirstOrDefault();
        }
    }
}

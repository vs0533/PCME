using MediatR;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class SignUpForUnitCreateOrUpdate: IRequest<SignUpForUnit>
    {
        /// <summary>
        /// 报名表编号 单位ID+流水号
        /// </summary>
        public string Code { get; private set; }
        public int WorkUnitId { get; private set; }

        public int TrainingCenterId { get; private set; }

        private readonly List<SignUpCollectionDto> _signUpCollection;
        public IReadOnlyCollection<SignUpCollectionDto> SignUpCollection => _signUpCollection;
        public bool IsPay { get; private set; }

        public bool IsLock { get; private set; }

        public DateTime CreateTime { get; private set; }

        public SignUpForUnitCreateOrUpdate(int workUnitId, int trainingCenterId, bool isPay, bool isLock)
        {
            _signUpCollection = new List<SignUpCollectionDto>();
            WorkUnitId = workUnitId;
            TrainingCenterId = trainingCenterId;
            IsPay = isPay;
            IsLock = isLock;
        }
        public void AddSignUpCollection(int studentId, int examSubjectId)
        {
            _signUpCollection.Add(new SignUpCollectionDto(studentId, examSubjectId));
        }
    }
    public class SignUpCollectionDto {
        public int StudentId { get; private set; }
        public int ExamSubjectId { get; private set; }

        public SignUpCollectionDto(int studentId, int examSubjectId)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
        }
    }
}

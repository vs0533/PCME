using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    /// <summary>
    /// 报名表
    /// </summary>
    public class SignUpForUnit:Entity, IAggregateRoot
    {
        /// <summary>
        /// 报名表编号 单位ID+流水号
        /// </summary>
        public string Code { get; private set; }
        public int WorkUnitId { get; private set; }
        public WorkUnit WorkUnit { get; private set; }

        public int TrainingCenterId { get; private set; }
        public TrainingCenter TrainingCenter { get; private set; }

        private readonly List<SignUpCollection> _signUpCollection;
        public IReadOnlyCollection<SignUpCollection> SignUpCollection => _signUpCollection;
        public bool IsPay { get; private set; }

        public bool IsLock { get; private set; }

        public DateTime CreateTime { get; private set; }

        public SignUpForUnit()
        {
            _signUpCollection = new List<SignUpCollection>();
        }

        public SignUpForUnit(string code, int workUnitId, int trainingCenterId, bool isPay, bool isLock)
        {
            _signUpCollection = new List<SignUpCollection>();
            Code = code;
            WorkUnitId = workUnitId;
            TrainingCenterId = trainingCenterId;
            IsPay = isPay;
            IsLock = isLock;
            CreateTime = DateTime.Now;
        }

        public void AddSignUpCollection(int studentId, int examSubjectId)
        {
            var isExists = _signUpCollection.Where(c => c.ExamSubjectId == examSubjectId && c.StudentId == studentId).FirstOrDefault();
            if (isExists == null)
            {
                SignUpCollection signUpCollection = new SignUpCollection(studentId, examSubjectId);
                _signUpCollection.Add(signUpCollection);
            }
            else {
                throw new Exception("该人员已经存在报名表内");
            }
        }
    }
}

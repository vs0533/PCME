using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpStudentAggregates
{
    public class SignUpStudent : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public int StudentId { get; private set; }
        public int TrainingCenterId { get; private set; }
        public DateTime CreateTime { get; private set; }
        private readonly List<SignUpStudentCollection> _signUpStudentCollection;
        public IReadOnlyCollection<SignUpStudentCollection> Collection => _signUpStudentCollection;
        public bool IsPay{ get; private set; }

        /// <summary>
        /// 支付置为成功
        /// </summary>
        public void PayToSuccess()
        {
            IsPay = true;
        }
        public SignUpStudent()
        {
            _signUpStudentCollection = new List<SignUpStudentCollection>();
        }
        public void AddCollection(int examsubjectid)
        {
            var existed = _signUpStudentCollection.FirstOrDefault(c => c.ExamSubjectId == examsubjectid);
            if (existed != null)
            {
                throw new Exception("科目已经选择");
            }
            else
            {
                _signUpStudentCollection.Add(new SignUpStudentCollection(
                 examsubjectid
                ));
            }
        }

        public SignUpStudent(string code, int studentId, int trainingCenterId, DateTime createTime)
        {
            _signUpStudentCollection = new List<SignUpStudentCollection>();
            Code = code ?? throw new ArgumentNullException(nameof(code));
            StudentId = studentId;
            TrainingCenterId = trainingCenterId;
            CreateTime = createTime;
            IsPay = false;
        }

        public void Update(string code, int studentId, int trainingCenterId,IEnumerable<SignUpStudentCollection> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            _signUpStudentCollection.Clear();
            foreach (var item in collection)
            {
                AddCollection(item.ExamSubjectId);
            }
            Code = code ?? throw new ArgumentNullException(nameof(code));
            StudentId = studentId;
            TrainingCenterId = trainingCenterId;
        }
    }
}

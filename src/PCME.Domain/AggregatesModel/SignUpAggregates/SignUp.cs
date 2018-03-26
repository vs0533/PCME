using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    public class SignUp:Entity, IAggregateRoot
    {
        public int StudentId { get; private set; }
        public int ExamSubjectId { get; private set; }
        public SignUpCollection SignUpCollection { get; private set; }
        public bool IsPay { get; private set; }

        public SignUp(int studentId, int examSubjectId, SignUpCollection signUpCollection, bool isPay=false)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
            SignUpCollection = signUpCollection ?? throw new ArgumentNullException(nameof(signUpCollection));
            IsPay = isPay;
        }
    }
}

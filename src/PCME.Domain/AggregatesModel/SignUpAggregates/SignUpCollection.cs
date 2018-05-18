using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    public class SignUpCollection:Entity,IAggregateRoot
    {
        public int StudentId { get; private set; }
        public Student Student { get; private set; }
        public int ExamSubjectId { get; private set; }
        public ExamSubject ExamSubject { get; private set; }
        public int SignUpForUnitId { get; private set; }
        public SignUpForUnit SignUpForUnit { get; private set; }

        public SignUpCollection()
        {

        }

        public SignUpCollection(int studentId,int examSubjectId, int signUpForUnitId)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
            SignUpForUnitId = signUpForUnitId;
        }
        public SignUpCollection(int studentId, int examSubjectId)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
        }
    }
}

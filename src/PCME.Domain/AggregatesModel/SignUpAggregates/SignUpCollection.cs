using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    public class SignUpCollection:Entity
    {
        public int StudentId { get; private set; }
        public int ExamSubjectId { get; private set; }

        public SignUpCollection()
        {

        }

        public SignUpCollection(int studentId,int examSubjectId)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
        }
    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpStudentAggregates
{
    public class SignUpStudentCollection:Entity
    {
        public int SignUpStudentId { get; private set; }
        public int ExamSubjectId { get; private set; }
        public SignUpStudent SignUpStudent { get; private set; }
        public SignUpStudentCollection()
        {

        }
        public SignUpStudentCollection(int examSubjectId)
        {
            ExamSubjectId = examSubjectId;
        }
    }
}

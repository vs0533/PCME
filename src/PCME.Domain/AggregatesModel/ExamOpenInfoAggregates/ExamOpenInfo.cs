using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamOpenInfoAggregates
{
    public class ExamOpenInfo:Entity,IAggregateRoot
    {
        public int TrainingCenterId { get; private set; }
        public int ExamSubjectId { get; private set; }

        public DateTime SignUpTime { get; private set; }
        public DateTime SignUpFinishTime { get; private set; }
        public int SignUpFinishOffset { get; private set; }

        public StudentAggregates.StudentType StudentType { get; private set; }

        public ExamOpenInfo()
        {

        }
        public ExamOpenInfo(int trainingCenterId,int examSubjectId,
            DateTime signUpTime,DateTime signUpFinishTime,
            StudentAggregates.StudentType studentType,int signUpFinishOffset = 0)
        {
            TrainingCenterId = trainingCenterId;
            ExamSubjectId = examSubjectId;
            SignUpTime = signUpTime;
            SignUpFinishTime = signUpFinishTime;
            StudentType = studentType;
            SignUpFinishOffset = signUpFinishOffset;
        }
    }
}

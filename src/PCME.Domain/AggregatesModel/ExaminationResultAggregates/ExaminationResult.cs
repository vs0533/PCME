using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationResultAggregates
{
    public class ExaminationResult:Entity
    {
        public int StudentId { get; private set; }
        public float Score { get; private set; }
        public string AdmissionTicketNum { get; private set; }
        public int SignUpId { get; private set; }
        public DateTime CreateTime { get; private set; }

        public ExaminationResult(int studentId, float score, string admissionTicketNum, int signUpId, DateTime createTime)
        {
            StudentId = studentId;
            Score = score;
            AdmissionTicketNum = admissionTicketNum ?? throw new ArgumentNullException(nameof(admissionTicketNum));
            SignUpId = signUpId;
            CreateTime = createTime;
        }
    }
}

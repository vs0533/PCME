using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamResultAggregates
{
    public class ExamResult:Entity,IAggregateRoot
    {
        public string TicketNum { get; private set; }
        public int AdmissionTicketId { get; private set; }
        public int StudentId { get; private set; }
        public int ExamSubjectId { get; private set; }
        public float Score { get; private set; }
        public DateTime CreateTime { get; private set; }
        public bool IstoExamAudit { get; private set; }
        public ExamResult()
        {

        }

        public ExamResult(string ticketNum, int admissionTicketId, int studentId, int examSubjectId, float score, DateTime createTime,bool istoexamaudit)
        {
            TicketNum = ticketNum ?? throw new ArgumentNullException(nameof(ticketNum));
            AdmissionTicketId = admissionTicketId;
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
            Score = score;
            CreateTime = DateTime.Now;
            IstoExamAudit = istoexamaudit;
        }
    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CreditExamAggregates
{
    public class CreditExam:Entity,IAggregateRoot
    {
        public string AdmissionTicketNum { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public float SumResult { get; set; }
        public float Credit { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationAggregates
{
    public class Examination:Entity,IAggregateRoot
    {
        public string ExamSubjectId { get; private set; }
        public int HowLong { get; private set; }
        public readonly List<TestPaper> _testPapers;
        public IReadOnlyCollection<TestPaper> TestPapers => _testPapers;

        public Examination(string examSubjectId, int howLong)
        {
            ExamSubjectId = examSubjectId ?? throw new ArgumentNullException(nameof(examSubjectId));
            HowLong = howLong;
        }
    }
}

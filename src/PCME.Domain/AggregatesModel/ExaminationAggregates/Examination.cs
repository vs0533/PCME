using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationAggregates
{
    public class Examination:Entity
    {
        public string ExamSubjectId { get; private set; }
        public int HowLong { get; private set; }
        public QuestionsType QuestionsType { get; private set; }

    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationAggregates
{
    public class TestPaper:Entity
    {
        public int ExamSubjectId { get; private set; }
        public QuestionsType QuestionsType { get; private set; }
        public int ReadCount { get; private set; }
        public int Score { get; private set; }

        public TestPaper(int examSubjectId, QuestionsType questionsType, int readCount, int score)
        {
            ExamSubjectId = examSubjectId;
            QuestionsType = questionsType ?? throw new ArgumentNullException(nameof(questionsType));
            ReadCount = readCount;
            Score = score;
        }
    }
}

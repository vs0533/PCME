using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.TestAggregates
{
    /// <summary>
    /// 试卷设置 试卷主要 有题型/出题量/每题分值的设置
    /// </summary>
    public class TestPaper:Entity,IAggregateRoot
    {
        public int TestTypeId { get; private set; }
        public TestType TestType { get; private set; }
        public int DisplayCount { get; private set; }
        public float Score { get; private set; }
        public int OrderBy { get; private set; }
        public int TestConfigId { get; private set; }
        public TestConfig TestConfig { get; private set; }

        public TestPaper()
        {

        }

        public TestPaper(int testTypeId, int displayCount, float score, int orderBy)
        {
            TestTypeId = testTypeId;
            DisplayCount = displayCount;
            Score = score;
            OrderBy = orderBy;
        }
    }
}

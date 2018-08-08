using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.TestAggregates
{
    /// <summary>
    /// 考试设置 考试主要包括 科目设置
    /// </summary>
    public class TestConfig : Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string CategoryCode { get; private set; }
        private readonly List<TestPaper> testPaper;
        public IReadOnlyCollection<TestPaper> TestPaper => testPaper;

        public TestConfig()
        {
            testPaper = new List<TestPaper>();
        }

        public TestConfig(string title, string categorycode)
        {
            testPaper = new List<TestPaper>();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            CategoryCode = categorycode;
        }

        public void AddTestPaper(int testTypeId, int displayCount, float score, int orderBy)
        {
            var existed = testPaper.FirstOrDefault(c => c.TestTypeId == testTypeId);
            if (existed != null)
            {
                throw new Exception("已经存在该题型的设置");
            }
            testPaper.Add(new TestPaper(
                testTypeId,
                displayCount,
                score,
                orderBy
            ));
        }
    }
}

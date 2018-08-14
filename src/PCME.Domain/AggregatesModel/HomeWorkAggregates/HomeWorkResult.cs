using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.HomeWorkAggregates
{
    public class HomeWorkResult:Entity,IAggregateRoot
    {
        public int StudentId { get; private set; }
        public float Score { get; private set; }
        public string CategoryCode { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime UpdateTime { get; private set; }
        public HomeWorkResult()
        {

        }

        public HomeWorkResult(int studentId, float score, string categoryCode, DateTime createTime, DateTime updateTime)
        {
            StudentId = studentId;
            Score = score;
            CategoryCode = categoryCode ?? throw new ArgumentNullException(nameof(categoryCode));
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
    }
}

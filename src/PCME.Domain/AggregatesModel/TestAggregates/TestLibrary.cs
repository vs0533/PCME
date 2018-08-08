using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.TestAggregates
{
    public class TestLibrary:Entity,IAggregateRoot
    {
        public string Topic { get; private set; }
        public string SelectItem { get; private set; }
        public string Answer { get; private set; }
        /// <summary>
        /// examsubjectcode
        /// </summary>
        public string CategoryCode { get; private set; }
        public int TestTypeId { get; private set; }
        public TestType TestType { get; private set; }
        public bool IsHomeWork { get; private set; }
        public TestLibrary()
        {

        }
    }
}

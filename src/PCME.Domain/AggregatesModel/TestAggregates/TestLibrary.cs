using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public int orderby { get; set; }
        [NotMapped]
        public float score { get; set; }

        public TestLibrary()
        {

        }

        public TestLibrary(string topic, string selectItem, string answer, string categoryCode, int testTypeId, bool isHomeWork)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            SelectItem = selectItem;
            Answer = answer ?? throw new ArgumentNullException(nameof(answer));
            CategoryCode = categoryCode ?? throw new ArgumentNullException(nameof(categoryCode));
            TestTypeId = testTypeId;
            IsHomeWork = isHomeWork;
        }
    }
}

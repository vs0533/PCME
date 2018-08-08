using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.TestAggregates
{
    public class TestType:Enumeration
    {
        public static TestType SingleChoice = new TestType(1, "单选题");
        public static TestType MultipleChoice = new TestType(2, "多选题");
        public static TestType GapFilling = new TestType(3, "填空题");
        public static TestType TrueOrFalse = new TestType(4, "判断题");
        public static TestType ShortAnswerQuestion = new TestType(5, "简答题");


        protected TestType()
        {
        }

        public TestType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TestType> List() =>
            new[] { SingleChoice, MultipleChoice, GapFilling,TrueOrFalse,ShortAnswerQuestion };

        public static TestType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TestType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TestType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TestType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

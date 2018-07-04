using System;
using System.Collections.Generic;
using System.Linq;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class PromoteType:Enumeration
    {
		public static PromoteType Review = new PromoteType(1, "评审");
		public static PromoteType Exam = new PromoteType(2, "考试");
        public static PromoteType WaitConfirmation = new PromoteType(3, "待定");
        public static IEnumerable<PromoteType> List() => new[] { Review, Exam, WaitConfirmation };
		public PromoteType()
        {

        }
		public PromoteType(int id, string name) :
            base(id, name)
        {

        }

		public static PromoteType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
		public static PromoteType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

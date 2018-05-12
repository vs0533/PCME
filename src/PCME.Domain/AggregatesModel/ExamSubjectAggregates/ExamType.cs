using System;
using System.Collections.Generic;
using System.Linq;
using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.ExamSubjectAggregates
{
	public class ExamType:Enumeration
    {
		public static ExamType Scene = new ExamType(1, "现场考试");
		public static ExamType NoScene = new ExamType(2, "非集中考试");
		public static IEnumerable<ExamType> List() => new[] { Scene, NoScene };
		public ExamType()
        {

        }
		public ExamType(int id, string name) :
            base(id, name)
        {

        }

		public static ExamType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExamSubjectDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
		public static ExamType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ExamSubjectDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

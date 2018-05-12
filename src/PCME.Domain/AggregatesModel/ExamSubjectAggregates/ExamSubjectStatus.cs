using System;
using System.Collections.Generic;
using System.Linq;
using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.ExamSubjectAggregates
{
	public class ExamSubjectStatus:Enumeration
    {
		public static ExamSubjectStatus Default = new ExamSubjectStatus(1, "可开设");
		public static ExamSubjectStatus Forbidden = new ExamSubjectStatus(2, "禁用");
		public static IEnumerable<ExamSubjectStatus> List() => new[] { Default, Forbidden };
		public ExamSubjectStatus()
        {

        }
		public ExamSubjectStatus(int id, string name) :
            base(id, name)
        {

        }

		public static ExamSubjectStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExamSubjectDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
		public static ExamSubjectStatus From(int id)
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

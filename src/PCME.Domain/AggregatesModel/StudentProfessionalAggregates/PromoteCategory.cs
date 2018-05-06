using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentProfessionalAggregates
{
    public class PromoteCategory:Enumeration
    {
        public static PromoteCategory Appraisal = new PromoteCategory(1, "评审");
        public static PromoteCategory Exam = new PromoteCategory(2, "考试");
        public static IEnumerable<PromoteCategory> List() => new[] { Appraisal, Exam };
        public PromoteCategory()
        {

        }
        public PromoteCategory(int id, string name) :
            base(id, name)
        {

        }

        public static PromoteCategory FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static PromoteCategory From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

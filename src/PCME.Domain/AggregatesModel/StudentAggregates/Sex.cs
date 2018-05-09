using PCME.Domain.SeedWork;
using PCME.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
    public class Sex:Enumeration
    {
        public static Sex Man = new Sex(1, "男");
        public static Sex Woman = new Sex(2, "女");
        public static Sex Unknown = new Sex(3, "未知");

        protected Sex()
        {
        }

        public Sex(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<Sex> List() =>
            new[] { Man, Woman,Unknown };

        public static Sex FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Sex From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

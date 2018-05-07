using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
    public class StudentStatus:Enumeration
    {
        public static StudentStatus Normal = new StudentStatus(1, "正常");
        public static StudentStatus Retire = new StudentStatus(2, "退休");
        public static StudentStatus Resign = new StudentStatus(3, "辞职");
        public static StudentStatus BeNotIn = new StudentStatus(4, "不在岗");
        public static IEnumerable<StudentStatus> List() => new[] { Normal, Retire, Resign, BeNotIn };
        public StudentStatus()
        {

        }
        public StudentStatus(int id, string name) :
            base(id, name)
        {

        }

        public static StudentStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static StudentStatus From(int id)
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

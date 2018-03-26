using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
    public class StudentType:Enumeration
    {
        public static StudentType Professional = new StudentType(1,nameof(Professional));
        public static StudentType CivilServant = new StudentType(2,nameof(CivilServant));
        public static IEnumerable<StudentType> List() => new[] { Professional, CivilServant };
        public StudentType()
        {

        }
        public StudentType(int id,string name):
            base(id,name)
        {

        }

        public static StudentType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name,name,StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new StudentDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static StudentType From(int id)
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

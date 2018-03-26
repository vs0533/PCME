using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamSubjectAggregates
{
    public class OpenType: Enumeration
    {
        public static OpenType Professional = new OpenType(1, nameof(Professional));
        public static OpenType CivilServant = new OpenType(2, nameof(CivilServant));
        public static IEnumerable<OpenType> List() => new[] { Professional, CivilServant };
        public OpenType()
        {

        }
        public OpenType(int id, string name) :
            base(id, name)
        {

        }

        public static OpenType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExamSubjectDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static OpenType From(int id)
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

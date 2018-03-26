using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationAggregates
{
    public class QuestionsType : Enumeration
    {
        public static QuestionsType JgUnit = new QuestionsType(1, nameof(JgUnit));
        public static QuestionsType SyUnit = new QuestionsType(2, nameof(SyUnit));
        public static QuestionsType Company = new QuestionsType(3, nameof(Company));
        public static IEnumerable<QuestionsType> List() => new[] { JgUnit, SyUnit, Company };
        public QuestionsType()
        {

        }
        public QuestionsType(int id, string name) :
            base(id, name)
        {

        }

        public static QuestionsType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExaminationDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static QuestionsType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ExaminationDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

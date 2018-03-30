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
        public static QuestionsType SingleChoice = new QuestionsType(1, nameof(SingleChoice));
        public static QuestionsType MultipleChoice = new QuestionsType(2, nameof(MultipleChoice));
        public static QuestionsType GapFilling = new QuestionsType(3, nameof(GapFilling));
        public static QuestionsType Judge = new QuestionsType(4, nameof(Judge));
        public static QuestionsType EssayQuestions = new QuestionsType(5, nameof(EssayQuestions));
        public static IEnumerable<QuestionsType> List() => new[] { SingleChoice, MultipleChoice, GapFilling,Judge,EssayQuestions };
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

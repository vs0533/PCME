using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.PaperAggregates
{
    public class AwardPaperLevel:Enumeration
    {
        public static AwardPaperLevel None = new AwardPaperLevel(1, "无等次");
        public static AwardPaperLevel One = new AwardPaperLevel(2, "一等奖");
        public static AwardPaperLevel Two = new AwardPaperLevel(3, "二等奖");
        public static AwardPaperLevel Three = new AwardPaperLevel(4, "三等奖");

        protected AwardPaperLevel()
        {
        }

        public AwardPaperLevel(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<AwardPaperLevel> List() =>
            new[] { None, One, Two, Three };

        public static AwardPaperLevel FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static AwardPaperLevel From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

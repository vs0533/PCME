using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.ScientificPayoffsAggregates
{
    public class AwardSPLevel:Enumeration
    {
        public static AwardSPLevel Evaluation = new AwardSPLevel(1, "鉴定成果");
        public static AwardSPLevel One = new AwardSPLevel(2, "一等奖");
        public static AwardSPLevel Two = new AwardSPLevel(3, "二等奖");
        public static AwardSPLevel Three = new AwardSPLevel(4, "三等奖");

        protected AwardSPLevel()
        {
        }

        public AwardSPLevel(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<AwardSPLevel> List() =>
            new[] { Evaluation, One, Two, Three };

        public static AwardSPLevel FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static AwardSPLevel From(int id)
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

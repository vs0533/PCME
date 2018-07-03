using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.PaperAggregates
{
    public class AreaLevel:Enumeration
    {
        public static AreaLevel City = new AreaLevel(1, "地市级");
        public static AreaLevel Province = new AreaLevel(2, "省部级");
        public static AreaLevel Country = new AreaLevel(3, "国家级");
        public static AreaLevel International = new AreaLevel(4, "国际级");

        protected AreaLevel()
        {
        }

        public AreaLevel(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<AreaLevel> List() =>
            new[] { City, Province, Country, International };

        public static AreaLevel FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static AreaLevel From(int id)
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

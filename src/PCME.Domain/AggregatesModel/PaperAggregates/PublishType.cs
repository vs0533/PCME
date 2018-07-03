using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.PaperAggregates
{
    /// <summary>
    /// 发表类型
    /// </summary>
    public class PublishType: Enumeration
    {
        public static PublishType Compile = new PublishType(1, "编著");
        public static PublishType Treatise = new PublishType(2, "专著");
        public static PublishType Paper = new PublishType(3, "论文");
        public static PublishType ExchangePaper = new PublishType(4, "交流文论");

        protected PublishType()
        {
        }

        public PublishType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<PublishType> List() =>
            new[] { Compile, Treatise, Paper,ExchangePaper };

        public static PublishType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for sex: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static PublishType From(int id)
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

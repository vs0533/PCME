using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates
{
    public class AuditStatus: Enumeration
    {
        public static AuditStatus Default = new AuditStatus(1, nameof(Default));
        public static AuditStatus Pass = new AuditStatus(2, nameof(Pass));
        public static AuditStatus VoteDown = new AuditStatus(3, nameof(VoteDown));

        public static IEnumerable<AuditStatus> List() => new[] { Default, Pass, VoteDown };
        public AuditStatus()
        {

        }
        public AuditStatus(int id, string name) :
            base(id, name)
        {

        }

        public static AuditStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExaminationRoomPlanDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static AuditStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ExaminationRoomPlanDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

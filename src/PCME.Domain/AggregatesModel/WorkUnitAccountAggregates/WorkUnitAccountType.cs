using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.WorkUnitAccountAggregates
{
    public class WorkUnitAccountType: Enumeration
    {
        /// <summary>
        /// Continuing Education
        /// </summary>
        public static WorkUnitAccountType Manager = new WorkUnitAccountType(1, "单位管理员");
        public static WorkUnitAccountType Approve = new WorkUnitAccountType(2, "单位审核");
        /// <summary>
        /// Continuing Education
        /// </summary>
        public static WorkUnitAccountType CE = new WorkUnitAccountType(3, "继续教育培训");
        /// <summary>
        /// CivilServant
        /// </summary>
        public static WorkUnitAccountType CS = new WorkUnitAccountType(4, "公务员培训");
        public static IEnumerable<WorkUnitAccountType> List() => new[] { Manager,Approve, CE, CS };
        public WorkUnitAccountType()
        {

        }
        public WorkUnitAccountType(int id, string name) :
            base(id, name)
        {

        }

        public static WorkUnitAccountType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new UnitDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static WorkUnitAccountType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new UnitDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{

    public class WorkUnitNature: Enumeration
    {
        public static WorkUnitNature JgUnit = new WorkUnitNature(1, "机关单位");
        public static WorkUnitNature SyUnit = new WorkUnitNature(2, "事业单位");
        public static WorkUnitNature Company = new WorkUnitNature(3, "企业");
        public static WorkUnitNature Unknown = new WorkUnitNature(4, "未知");
        public static IEnumerable<WorkUnitNature> List() => new[] { JgUnit, SyUnit, Company, Unknown };
        public WorkUnitNature()
        {

        }
        public WorkUnitNature(int id, string name) :
            base(id, name)
        {

        }

        public static WorkUnitNature FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new UnitDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static WorkUnitNature From(int id)
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

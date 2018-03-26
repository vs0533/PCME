using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{

    public class UnitNature: Enumeration
    {
        public static UnitNature JgUnit = new UnitNature(1, nameof(JgUnit));
        public static UnitNature SyUnit = new UnitNature(2, nameof(SyUnit));
        public static UnitNature Company = new UnitNature(3, nameof(Company));
        public static IEnumerable<UnitNature> List() => new[] { JgUnit, SyUnit, Company };
        public UnitNature()
        {

        }
        public UnitNature(int id, string name) :
            base(id, name)
        {

        }

        public static UnitNature FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new UnitDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static UnitNature From(int id)
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

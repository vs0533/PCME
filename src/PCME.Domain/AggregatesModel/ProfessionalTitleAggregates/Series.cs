using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ProfessionalTitleAggregates
{
    public class Series:Entity
    {
        public string Name { get; private set; }

        public Series(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ProfessionalTitleAggregates
{
    public class Specialty:Entity
    {
        public string Name { get; private set; }
        public Specialty()
        {

        }

        public Specialty(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}

using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ProfessionalTitleAggregates
{
    public class Level:Entity
    {
        public string Name { get; private set; }

        public Level()
        {

        }
        public Level(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}

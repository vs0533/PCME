using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ProfessionalTitleAggregates
{
	public class Series:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public Series()
        {

        }
        public Series(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}

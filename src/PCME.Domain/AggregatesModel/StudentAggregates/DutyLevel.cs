using System;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class DutyLevel:Entity
    {
        public string Name { get; private set; }

        public DutyLevel()
        {

        }

		public DutyLevel(string name)
		{
			Name = name;
		}
	}

}

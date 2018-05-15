using System;
using System.Collections.Generic;
using System.Linq;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.TrainingCenterAggregates
{
	public class TrainingCenterType:Enumeration
    {
		/// <summary>
        /// Continuing Education
        /// </summary>
		public static TrainingCenterType CE = new TrainingCenterType(3, "继续教育培训");
        /// <summary>
        /// CivilServant
        /// </summary>
		public static TrainingCenterType CS = new TrainingCenterType(4, "公务员培训");
		public static IEnumerable<TrainingCenterType> List() => new[] { CE, CS };
		public TrainingCenterType()
        {

        }
		public TrainingCenterType(int id, string name) :
            base(id, name)
        {

        }

		public static TrainingCenterType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
		public static TrainingCenterType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            
            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}

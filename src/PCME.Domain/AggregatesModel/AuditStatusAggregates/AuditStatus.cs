using System;
using System.Collections.Generic;
using System.Linq;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.AuditStatusAggregates
{
	public class AuditStatus : Enumeration
	{
		public static AuditStatus Wait = new AuditStatus(1, "待审核");
		public static AuditStatus Pass = new AuditStatus(2, "通过");
		public static AuditStatus Veto = new AuditStatus(2, "否决");

		public static IEnumerable<AuditStatus> List() => new[] { Wait, Pass, Veto };
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
				throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}
		public static AuditStatus From(int id)
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

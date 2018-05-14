using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamSubjectAggregates
{
	public class ExamSubject : Entity, IAggregateRoot
	{
		/// <summary>
		/// 科目代码
		/// </summary>
		/// <value>The code.</value>
		public string Code
		{
			get;
			private set;
		}
		public string Name { get; private set; }
		public OpenType OpenType { get; private set; }
		public ExamType ExamType { get; private set; }
		public ExamSubjectStatus ExamSubjectStatus { get; private set; }
        public Series Series { get; private set; }
		public int OpenTypeId { get; private set; }
		public int ExamTypeId { get; private set; }
		public int? SeriesId { get; private set; }
        public int ExamSubjectStatusId { get; private set; }

		/// <summary>
        /// 面授次数
        /// </summary>
        /// <value>The MSC ount.</value>      
        public int MSCount { get; private set; }
        /// <summary>
        /// 开设系列
        /// </summary>
        /// <value>The series identifier.</value>

		public int CreditHour { get; private set; }

        public ExamSubject()
		{

		}

		public ExamSubject(string code, string name, int openTypeId, int examTypeId, int? seriesId, int examSubjectStatusId, int mSCount, int creditHour)
		{
			Code = code;
			Name = name;
			OpenTypeId = openTypeId;
			ExamTypeId = examTypeId;
			SeriesId = seriesId;
			ExamSubjectStatusId = examSubjectStatusId;
			MSCount = mSCount;
			CreditHour = creditHour;
		}
	}
}
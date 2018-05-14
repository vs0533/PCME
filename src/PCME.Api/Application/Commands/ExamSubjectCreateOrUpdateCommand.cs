using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;

namespace PCME.Api.Application.Commands
{
	public class ExamSubjectCreateOrUpdateCommand:IRequest<ExamSubject>
    {
		
		public int Id { get; private set; }
		/// <summary>
        /// 科目代码
        /// </summary>
        /// <value>The code.</value>
        [Required(ErrorMessage ="科目代码必须填写")]
        public string Code
        {
            get;
            private set;
        }
		[Required(ErrorMessage = "科目名称必须填写")]
        public string Name { get; private set; }
        public OpenType OpenType { get; private set; }
        public ExamType ExamType { get; private set; }
        public ExamSubjectStatus ExamSubjectStatus { get; private set; }
        public Series Series { get; private set; }

		[Required(ErrorMessage = "科目类型必须选择")]
		[JsonProperty("OpenType.Id")]
        public int OpenTypeId { get; private set; }

		[Required(ErrorMessage = "考试类型必须选择")]
		[JsonProperty("ExamType.Id")]
        public int ExamTypeId { get; private set; }

		[JsonProperty("Series.Id")]
        public int? SeriesId { get; private set; }

		[Required(ErrorMessage = "科目状态必须选择")]
		[JsonProperty("ExamSubjectStatus.Id")]
        public int ExamSubjectStatusId { get; private set; }

        /// <summary>
        /// 面授次数
        /// </summary>
        /// <value>The MSC ount.</value>
		[Required(ErrorMessage = "面授次数必须填写")]
        public int MSCount { get; private set; }
        /// <summary>
        /// 开设系列
        /// </summary>
        /// <value>The series identifier.</value>
		[Required(ErrorMessage = "学分必须填写")]
        public int CreditHour { get; private set; }

		public ExamSubjectCreateOrUpdateCommand(int id,string code, string name, int openTypeId, int examTypeId, int? seriesId, int examSubjectStatusId, int mSCount, int creditHour)
		{
			Id = id;
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

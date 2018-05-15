using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;

namespace PCME.Api.Application.Commands
{
	public class ExamSubjectOpenInfoCreateOrUpdateCommand:IRequest<ExamSubjectOpenInfo>
    {
		public int Id { get; private set; }
		[JsonProperty("TrainingCenter.Id")]
		public int? TrainingCenterId { get; private set; }
        public TrainingCenter TrainingCenter { get; private set; }

		[Required(ErrorMessage ="申请科目必须选择")]
		[JsonProperty("ExamSubject.Id")]
        public int ExamSubjectId { get; private set; }
        public ExamSubject ExamSubject { get; set; }

        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// 报名开始时间
        /// </summary>
        /// <value>The sign up time.</value>
		[Required(ErrorMessage ="报名开始时间必须选择")]
        public DateTime SignUpTime { get; private set; }
        /// <summary>
        /// 报名结束时间
        /// </summary>
        /// <value>The sign up finish time.</value>
		[Required(ErrorMessage = "报名结束时间必须选择")]
        public DateTime SignUpFinishTime { get; private set; }
        /// <summary>
        /// 扫描结束时间偏移量
        /// </summary>
        /// <value>The sign up finish offset.</value>
		[Required(ErrorMessage = "扫描结束时间偏移量必须设置，如果不想设置可以输入0")]
        public int SignUpFinishOffset { get; private set; }

        /// <summary>
        /// 考试开始结束时间 字符串类型 仅用于提示作用
        /// </summary>
        /// <value>The display exam time.</value>
		[Required(ErrorMessage = "考试时间必须填写")]
        public string DisplayExamTime
        {
            get;
            private set;
        }
		[JsonProperty("AuditStatus.Id")]
        public int? AuditStatusId { get; private set; }

		public ExamSubjectOpenInfoCreateOrUpdateCommand(int id, int trainingCenterId, int examSubjectId, DateTime signUpTime, DateTime signUpFinishTime, int signUpFinishOffset, string displayExamTime, int auditStatusId)
		{
			Id = id;
			TrainingCenterId = trainingCenterId;
			ExamSubjectId = examSubjectId;
			SignUpTime = signUpTime;
			SignUpFinishTime = signUpFinishTime;
			SignUpFinishOffset = signUpFinishOffset;
			DisplayExamTime = displayExamTime;
			AuditStatusId = auditStatusId;
		}

        public void SetTrainingCenter(int trainingCenterId)
		{
			TrainingCenterId = trainingCenterId;
		}
		public void SetAuditStatus(int auditStatusId){
			AuditStatusId = auditStatusId;
		}
		public void SetId(int id){
			Id = id;
		}
	}
}

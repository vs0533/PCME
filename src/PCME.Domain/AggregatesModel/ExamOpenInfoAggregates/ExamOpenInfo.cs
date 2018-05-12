using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamOpenInfoAggregates
{
    public class ExamOpenInfo:Entity,IAggregateRoot
    {
        public int TrainingCenterId { get; private set; }
        public int ExamSubjectId { get; private set; }

        public DateTime SignUpTime { get; private set; }
        public DateTime SignUpFinishTime { get; private set; }
        public int SignUpFinishOffset { get; private set; }

        /// <summary>
        /// 考试开始结束时间 字符串类型 仅用于提示作用
        /// </summary>
        /// <value>The display exam time.</value>
        public string DisplayExamTime
		{
			get;
			private set;
		}

        public int AuditStatusId { get; private set; }
      
        public ExamOpenInfo()
        {

        }

		public ExamOpenInfo(int trainingCenterId, int examSubjectId, DateTime signUpTime, DateTime signUpFinishTime, int signUpFinishOffset, string displayExamTime, int auditStatusId)
		{
			TrainingCenterId = trainingCenterId;
			ExamSubjectId = examSubjectId;
			SignUpTime = signUpTime;
			SignUpFinishTime = signUpFinishTime;
			SignUpFinishOffset = signUpFinishOffset;
			DisplayExamTime = displayExamTime;
			AuditStatusId = auditStatusId;
		}
	}
}

using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.SeedWork;
using System;

namespace PCME.Domain.AggregatesModel.ExamOpenInfoAggregates
{
	public class ExamSubjectOpenInfo:Entity,IAggregateRoot
    {
        public int TrainingCenterId { get; private set; }
		public TrainingCenter TrainingCenter { get; private set; }

        public int ExamSubjectId { get; private set; }      
        public ExamSubject ExamSubject { get; set; }

        public AuditStatus AuditStatus { get; set; }
        
        /// <summary>
        /// 报名开始时间
        /// </summary>
        /// <value>The sign up time.</value>
        public DateTime SignUpTime { get; private set; }
        /// <summary>
        /// 报名结束时间
        /// </summary>
        /// <value>The sign up finish time.</value>
        public DateTime SignUpFinishTime { get; private set; }
        /// <summary>
        /// 扫描结束时间偏移量
        /// </summary>
        /// <value>The sign up finish offset.</value>
        public int SignUpFinishOffset { get; private set; }
        /// <summary>
        /// 去培训点扫描的时间
        /// </summary>
        public DateTime GoToValDateTime { get; private set; }
        /// <summary>
        /// 报名费
        /// </summary>
        public decimal Pirce { get; private set; }

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
      
        public ExamSubjectOpenInfo()
        {

        }

		public ExamSubjectOpenInfo(int trainingCenterId, int examSubjectId, DateTime signUpTime, DateTime signUpFinishTime, int signUpFinishOffset, string displayExamTime, int auditStatusId,decimal pirce,DateTime goToValDateTime)
		{
			TrainingCenterId = trainingCenterId;
			ExamSubjectId = examSubjectId;
			SignUpTime = signUpTime;
			SignUpFinishTime = signUpFinishTime;
			SignUpFinishOffset = signUpFinishOffset;
			DisplayExamTime = displayExamTime;
			AuditStatusId = auditStatusId;
            Pirce = pirce;
            GoToValDateTime = goToValDateTime;

        }

		public void Update(int trainingCenterId,int examSubjectId, DateTime signUpTime, DateTime signUpFinishTime, int signUpFinishOffset, string displayExamTime, int auditStatusId, decimal pirce, DateTime goToValDateTime)
		{
			TrainingCenterId = trainingCenterId;
			ExamSubjectId = examSubjectId;
            SignUpTime = signUpTime;
            SignUpFinishTime = signUpFinishTime;
            SignUpFinishOffset = signUpFinishOffset;
            DisplayExamTime = displayExamTime;
            AuditStatusId = auditStatusId;
            Pirce = pirce;
            GoToValDateTime = goToValDateTime;
        }

		public void SetTrainingCenter(int trainingCenterId){
			TrainingCenterId = trainingCenterId;
		}

		public void SetExamSubject(int examSubjectId){
			ExamSubjectId = examSubjectId;
		}

		public void ChangeAuditStatus(int auditStatudId){
			AuditStatusId = auditStatudId;
		}
	}
}
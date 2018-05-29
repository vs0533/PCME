using PCME.Domain.SeedWork;
using System;

namespace PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates
{
    public class ExaminationRoomPlan:Entity,IAggregateRoot
    {
        public int ExaminationRoomId { get; private set; }
        /// <summary>
        /// 由系统自动生成 yy+科目ID四位+序号两位
        /// </summary>
        public string Num { get; private set; }
        public DateTime SelectTime { get; private set; }
        public DateTime SelectFinishTime { get; private set; }
        public DateTime SignInTime { get; private set; }
        public DateTime ExamEndTime { get; private set; }
        public DateTime ExamStartTime { get; private set; }
        public int ExamSubjectID { get; private set; }
        //public AuditStatus AuditStatus { get; private set; }
        public int AuditStatusId { get; private set; }
        public int PlanStatusId { get; private set; }
        public int TrainingCenterId { get; private set; }
        public ExaminationRoomPlan()
        {

        }

        public ExaminationRoomPlan(int examinationRoomId, string num, DateTime selectTime, DateTime selectFinishTime, DateTime signInTime, DateTime examEndTime, DateTime examStartTime, int examSubjectID, int auditStatusId, int planStatusId, int trainingCenterId)
        {
            ExaminationRoomId = examinationRoomId;
            Num = num;
            SelectTime = selectTime;
            SelectFinishTime = selectFinishTime;
            SignInTime = signInTime;
            ExamEndTime = examEndTime;
            ExamStartTime = examStartTime;
            ExamSubjectID = examSubjectID;
            AuditStatusId = auditStatusId;
            PlanStatusId = planStatusId;
            TrainingCenterId = trainingCenterId;
        }
    }
}

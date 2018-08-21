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
        /// <summary>
        /// 场次人数
        /// </summary>
        public int Galleryful { get; set; }
        public DateTime SelectTime { get; private set; }
        public DateTime SelectFinishTime { get; private set; }
        public DateTime SignInTime { get; private set; }
        public DateTime ExamEndTime { get; private set; }
        public DateTime ExamStartTime { get; private set; }
        //public AuditStatus AuditStatus { get; private set; }
        public int AuditStatusId { get; private set; }
        public int PlanStatusId { get; private set; }
        public int TrainingCenterId { get; private set; }
        public ExaminationRoomPlan()
        {

        }

        public ExaminationRoomPlan(int examinationRoomId, string num,int galleryful, DateTime selectTime, DateTime selectFinishTime, DateTime signInTime, DateTime examEndTime, DateTime examStartTime, int auditStatusId, int planStatusId, int trainingCenterId)
        {
            ExaminationRoomId = examinationRoomId;
            Num = num;
            Galleryful = galleryful;
            SelectTime = selectTime;
            SelectFinishTime = selectFinishTime;
            SignInTime = signInTime;
            ExamEndTime = examEndTime;
            ExamStartTime = examStartTime;
            AuditStatusId = auditStatusId;
            PlanStatusId = planStatusId;
            TrainingCenterId = trainingCenterId;
        }

        public void Update(int examinationRoomId, int galleryful, DateTime selectTime, DateTime selectFinishTime, DateTime signInTime, DateTime examEndTime, DateTime examStartTime, int auditStatusId, int planStatusId)
        {
            ExaminationRoomId = examinationRoomId;
            Galleryful = galleryful;
            SelectTime = selectTime;
            SelectFinishTime = selectFinishTime;
            SignInTime = signInTime;
            ExamEndTime = examEndTime;
            ExamStartTime = examStartTime;
            AuditStatusId = auditStatusId;
            PlanStatusId = planStatusId;
        }
        public void StartExam(DateTime signInTime) {
            PlanStatusId = PlanStatus.SignInStart.Id;
            SignInTime = signInTime;
        }
        public void EndExam()
        {
            PlanStatusId = PlanStatus.Over.Id;
        }
    }
}

using PCME.Domain.SeedWork;
using System;

namespace PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates
{
    public class ExaminationRoomPlan:Entity,IAggregateRoot
    {
        public int ExaminationRoomId { get; private set; }
        public int Num { get; private set; }
        public DateTime SelectTime { get; private set; }
        public DateTime SelectFinishTime { get; private set; }
        public DateTime? SignInTime { get; private set; }
        public int SignInOffset { get; private set; }
        public DateTime ExamStartTime { get; private set; }

        public PlanStatus PlanStatus { get; private set; }
        //public AuditStatus AuditStatus { get; private set; }
        public int AuditStatusId { get; private set; }
        public ExaminationRoomPlan()
        {

        }

        public ExaminationRoomPlan(int examinationRoomId, int num,
            DateTime selectTime, DateTime selectFinishTime, int signInOffset, DateTime examStartTime,int auditStatusId)
            :this(examinationRoomId,num,selectTime,selectFinishTime,
                 null,signInOffset,examStartTime,PlanStatus.Default,auditStatusId)
        {

        }

        public ExaminationRoomPlan(int examinationRoomId,int num,
            DateTime selectTime,DateTime selectFinishTime,
            DateTime? signInTime,int signInOffset,DateTime examStartTime,
            PlanStatus planStatus,int auditStatusId)
        {
            ExaminationRoomId = examinationRoomId;
            Num = num;
            SelectTime = SelectTime;
            SelectFinishTime = selectFinishTime;
            SignInOffset = signInOffset;
            ExamStartTime = examStartTime;
            PlanStatus = planStatus;
            AuditStatusId = auditStatusId;
        }
    }
}

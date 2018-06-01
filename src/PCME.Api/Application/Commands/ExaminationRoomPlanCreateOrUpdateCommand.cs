using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomPlanCreateOrUpdateCommand:IRequest<ExaminationRoomPlan>
    {
        public int Id { get; private set; }
        public int ExaminationRoomId { get; private set; }
        /// <summary>
        /// 由系统自动生成 yy+科目ID四位+序号两位
        /// </summary>
        public string Num { get; private set; }
        [Required]
        public DateTime SelectTime { get; private set; }

        [Required]
        public DateTime SelectFinishTime { get; private set; }
        [Required]
        public DateTime SignInTime { get; private set; }
        [Required]
        public DateTime ExamEndTime { get; private set; }
        [Required]
        public DateTime ExamStartTime { get; private set; }
        [Required]
        public int ExamSubjectID { get; private set; }
        //public AuditStatus AuditStatus { get; private set; }
        public int AuditStatusId { get; private set; }
        public int PlanStatusId { get; private set; }
        public int TrainingCenterId { get; private set; }

        public ExaminationRoomPlanCreateOrUpdateCommand(int id, int examinationRoomId, string num, DateTime selectTime, DateTime selectFinishTime, DateTime signInTime, DateTime examEndTime, DateTime examStartTime, int examSubjectID, int auditStatusId, int planStatusId, int trainingCenterId)
        {
            Id = id;
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

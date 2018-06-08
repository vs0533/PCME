using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomPlanCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get; private set; }
        [JsonProperty("examinationrooms.Id")]
        public int ExaminationRoomId { get; private set; }
        /// <summary>
        /// 由系统自动生成 yy+科目ID四位+序号两位
        /// </summary>
        [JsonProperty("examinationroomplans.Num")]
        //[RegularExpression(@"^0?[0-9]{6}$", ErrorMessage = "编号必须是6位数字字符分别由两位年+两位日序号+两位场次序号 如:180101代表18年第一天的第一场考试")]
        public string Num { get; private set; }
        [JsonProperty("examinationroomplans.Galleryful")]
        [Required(ErrorMessage ="必须填写容纳人数")]
        public int Galleryful { get; private set; }
        [Required]
        [JsonProperty("examinationroomplans.SelectTime")]
        public DateTime SelectTime { get; private set; }

        [Required]
        [JsonProperty("examinationroomplans.SelectFinishTime")]
        public DateTime SelectFinishTime { get; private set; }
        [Required]
        [JsonProperty("examinationroomplans.SignInTime")]
        public DateTime SignInTime { get; private set; }
        [Required]
        [JsonProperty("examinationroomplans.ExamEndTime")]
        public DateTime ExamEndTime { get; private set; }
        [Required]
        [JsonProperty("examinationroomplans.ExamStartTime")]
        public DateTime ExamStartTime { get; private set; }
        //public AuditStatus AuditStatus { get; private set; }
        [JsonProperty("auditstatus.Id")]
        public int AuditStatusId { get; private set; }
        [JsonProperty("planstatus.Id")]
        public int PlanStatusId { get; private set; }
        public int TrainingCenterId { get; private set; }

        public void SetId(int id) {
            Id = id;
        }

        public void TrainingCenterToAddSetInfo(string num,int auditStatusId,int planStatusId,int trainingCenterId) {
            Num = num;
            AuditStatusId = auditStatusId;
            PlanStatusId = planStatusId;
            TrainingCenterId = trainingCenterId;
        }
        public void AdminToSetInfo(int auditStatusId, int planStatusId)
        {
            AuditStatusId = auditStatusId;
            PlanStatusId = planStatusId;
        }

        public ExaminationRoomPlanCreateOrUpdateCommand(int id, int examinationRoomId, string num, int galleryful, DateTime selectTime, DateTime selectFinishTime, DateTime signInTime, DateTime examEndTime, DateTime examStartTime, int auditStatusId, int planStatusId, int trainingCenterId)
        {
            Id = id;
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
    }
}

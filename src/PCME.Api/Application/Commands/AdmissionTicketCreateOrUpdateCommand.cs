using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.AdmissionTicketAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class AdmissionTicketCreateOrUpdateCommand:IRequest<AdmissionTicket>
    {
        public int Id { get; private set; }
        public string Num { get; private set; }
        public int StudentId { get; private set; }
        [JsonProperty("examinationroom.Id")]
        public int ExaminationRoomId { get; private set; }
        public int SignUpId { get; private set; }
        [JsonProperty("examsubject.Id")]
        public int ExamSubjectId { get; private set; }
        public DateTime? SignInTime { get; private set; }
        public DateTime? LoginTime { get; private set; }
        public DateTime? PostPaperTime { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int ExaminationRoomPlanId { get; private set; }

        public void SetId(int id) {
            Id = id;
        }
        public void CreateAppend(string num, int studentId) {
            Num = num;
            StudentId = studentId;
        }
        public AdmissionTicketCreateOrUpdateCommand(int examinationRoomId, int signUpId, int examSubjectId, int examinationRoomPlanId)
        {
            ExaminationRoomId = examinationRoomId;
            SignUpId = signUpId;
            ExamSubjectId = examSubjectId;
            ExaminationRoomPlanId = examinationRoomPlanId;
        }
    }
}

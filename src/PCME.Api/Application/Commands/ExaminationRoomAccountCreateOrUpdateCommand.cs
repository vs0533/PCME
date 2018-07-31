using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomAccountCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get; set; }
        [JsonProperty("examinationroomaccount.AccountName")]
        public string AccountName { get; set; }
        [JsonProperty("examinationroomaccount.Password")]
        public string Password { get; set; }
        public int TrainingCenterId { get; set; }
        [JsonProperty("examinationroom.Id")]
        public int ExaminationRoomId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

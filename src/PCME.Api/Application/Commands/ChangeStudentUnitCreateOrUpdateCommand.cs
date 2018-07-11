using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ChangeStudentUnitCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get;  set; }
        [JsonProperty("oldunit.Id")]
        public int OldUnitId { get;  set; }
        [JsonProperty("newunit.Id")]
        public int NewUnitId { get;  set; }
        [JsonProperty("student.Id")]
        public int StudentId { get;  set; }
        [JsonProperty("auditstatus.Id")]
        public int AuditStatusId { get;  set; }
        public DateTime CreateTime { get;  set; }
        public DateTime AuditStatusTime { get;  set; }
        [JsonProperty("student.IDCard")]
        public string IDcard { get; set; }
        public ChangeStudentUnitCreateOrUpdateCommand()
        {

        }
        public void SetAuditStatus(int auditstatudid) {
            AuditStatusId = auditstatudid;
        }
        public void SetId(int id) {
            Id = id;
        }

        public ChangeStudentUnitCreateOrUpdateCommand(string idcard,int id,int oldUnitId, int newUnitId, int studentId, int auditStatusId)
        {
            IDcard = idcard;
            Id = id;
            OldUnitId = oldUnitId;
            NewUnitId = newUnitId;
            StudentId = studentId;
            AuditStatusId = auditStatusId;
        }
    }
}

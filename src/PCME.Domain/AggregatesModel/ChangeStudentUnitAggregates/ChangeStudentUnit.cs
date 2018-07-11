using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ChangeStudentUnitAggregates
{
    public class ChangeStudentUnit:Entity,IAggregateRoot
    {
        public int OldUnitId { get; private set; }
        public int NewUnitId { get; private set; }
        public int StudentId { get; private set; }
        public int AuditStatusId { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime? AuditStatusTime { get; private set; }
        public ChangeStudentUnit()
        {

        }

        public ChangeStudentUnit(int oldUnitId, int newUnitId, int studentId, int auditStatusId,DateTime createtime)
        {
            OldUnitId = oldUnitId;
            NewUnitId = newUnitId;
            StudentId = studentId;
            AuditStatusId = auditStatusId;
            CreateTime = createtime;
        }
        public void Update (int oldUnitId, int newUnitId, int studentId, int auditStatusId,DateTime dateTime)
        {
            OldUnitId = oldUnitId;
            NewUnitId = newUnitId;
            StudentId = studentId;
            AuditStatusId = auditStatusId;
            CreateTime = dateTime;
        }
        public void Audit(int auditstatusid, DateTime audittime) {
            AuditStatusId = auditstatusid;
            AuditStatusTime = audittime;
        }
    }
}

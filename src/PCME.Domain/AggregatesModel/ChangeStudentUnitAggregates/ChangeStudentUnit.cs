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
        public DateTime AuditStatusTime { get; private set; }

        public ChangeStudentUnit(int oldUnitId, int newUnitId, int studentId, int auditStatusId)
        {
            OldUnitId = oldUnitId;
            NewUnitId = newUnitId;
            StudentId = studentId;
            AuditStatusId = auditStatusId;
        }
    }
}

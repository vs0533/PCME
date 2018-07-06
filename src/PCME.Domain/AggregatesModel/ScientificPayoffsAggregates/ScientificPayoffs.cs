using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ScientificPayoffsAggregates
{
    /// <summary>
    /// 科研成果
    /// </summary>
    public class ScientificPayoffs:Entity,IAggregateRoot
    {
        public string Name { get; set; }
        public DateTime ComplateDate { get; set; }
        public int AreaLevelId { get; set; }
        public int AwardSPLevelId { get; set; }
        /// <summary>
        /// 参与等级
        /// </summary>
        public int JoinLevel { get; set; }
        public float Credit { get; set; }
        public int AuditStateId { get; set; }
        public string AuditAccount { get; set; }
        public int StudentId { get; set; }
        public ScientificPayoffs()
        {

        }

        public ScientificPayoffs(string name, DateTime complateDate, int areaLevelId, int awardSPLevelId, int joinLevel, float credit, int auditStateId, string auditAccount, int studentId)
        {
            Name = name;
            ComplateDate = complateDate;
            AreaLevelId = areaLevelId;
            AwardSPLevelId = awardSPLevelId;
            JoinLevel = joinLevel;
            Credit = credit;
            AuditStateId = auditStateId;
            AuditAccount = auditAccount;
            StudentId = studentId;
        }
    }
}

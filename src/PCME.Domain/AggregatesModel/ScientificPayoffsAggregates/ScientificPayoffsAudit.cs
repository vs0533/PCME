using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ScientificPayoffsAggregates
{
    /// <summary>
    /// 科研成果
    /// </summary>
    public class ScientificPayoffsAudit:Entity,IAggregateRoot
    {
        public string Name { get; set; }
        public DateTime ComplateDate { get; set; }
        public int AreaLevelId { get; set; }
        public int AwardSPLevelId { get; set; }
        /// <summary>
        /// 参与等级
        /// </summary>
        public int JoinLevel { get; set; }
    }
}

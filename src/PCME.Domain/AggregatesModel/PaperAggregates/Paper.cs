﻿using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.PaperAggregates
{
    /// <summary>
    /// 著作论文实体类
    /// </summary>
    public class Paper:Entity,IAggregateRoot
    {
        public string Name { get; set; }
        /// <summary>
        /// 获奖等级
        /// </summary>
        public int AwardPaperLevelId { get; set; }
        /// <summary>
        /// 发表类型
        /// </summary>
        public int PublishTypeId { get; set; }
        /// <summary>
        /// 刊物
        /// </summary>
        public int PeriodicalId { get; set; }
        /// <summary>
        /// 参与级别
        /// </summary>
        public int JoinLevel { get; set; }
        public int JoinCount { get; set; }
        public float Credit { get; set; }
        public int AuditStatusId { get; set; }
        public string AuditAccount { get; set; }
        public DateTime PublishDate { get; set; }

        public Paper(string name, DateTime publishDate, int publishTypeId, int periodicalId, int joinLevel, int joinCount, float credit, int auditStatusId)
        {
            Name = name;
            PublishDate = publishDate;
            PublishTypeId = publishTypeId;
            PeriodicalId = periodicalId;
            JoinLevel = joinLevel;
            JoinCount = joinCount;
            Credit = credit;
            AuditStatusId = auditStatusId;
        }
    }
}

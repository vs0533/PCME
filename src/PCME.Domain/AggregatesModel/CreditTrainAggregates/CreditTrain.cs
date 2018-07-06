using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CreditTrainAggregates
{
    public class CreditTrain:Entity,IAggregateRoot
    {
        /// <summary>
        /// 主办单位
        /// </summary>
        public string Sponsor { get; set; }
        /// <summary>
        /// 培训内容
        /// </summary>
        public string TrainContent { get; set; }
        public DateTime TrainDate { get; set; }
        /// <summary>
        /// 培训类型
        /// </summary>
        public string TrainType { get; set; }
        public int AuditStatusId { get; set; }

        public string AuditAccount { get; set; }
        public int StudentId { get; set; }
        /// <summary>
        /// 培训学时
        /// </summary>
        public string Period { get; set; }
        public float Credit { get; set; }
        public CreditTrain()
        {

        }

        public CreditTrain(string sponsor, string trainContent, DateTime trainDate, string trainType, int auditStatusId, string auditAccount, int studentId, string period, float credit)
        {
            Sponsor = sponsor;
            TrainContent = trainContent;
            TrainDate = trainDate;
            TrainType = trainType;
            AuditStatusId = auditStatusId;
            AuditAccount = auditAccount;
            StudentId = studentId;
            Period = period;
            Credit = credit;
        }
    }
}

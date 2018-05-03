using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TrainAudit
    {
        public int TrainId { get; set; }
        public string PersonId { get; set; }
        public string FrontUnit { get; set; }
        public string TrainContent { get; set; }
        public DateTime TrainDate { get; set; }
        public string TrainType { get; set; }
        public string TrainPeriod { get; set; }
        public int CreditHour { get; set; }
        public int AuditState { get; set; }
        public string AuditAccount { get; set; }

        public PersonTechnician Person { get; set; }
    }
}

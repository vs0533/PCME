using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class FruitAudit
    {
        public int FruitId { get; set; }
        public string PersonId { get; set; }
        public string FruitName { get; set; }
        public DateTime CompleteDate { get; set; }
        public string AreaLevel { get; set; }
        public string AwardLevel { get; set; }
        public int JoinLevel { get; set; }
        public int CreditHour { get; set; }
        public int AuditState { get; set; }
        public string AuditAccount { get; set; }

        public PersonTechnician Person { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PaperAudit
    {
        public int PaperId { get; set; }
        public string PersonId { get; set; }
        public string PaperName { get; set; }
        public string PaperType { get; set; }
        public int? PublicationId { get; set; }
        public DateTime PublishDate { get; set; }
        public string AreaLevel { get; set; }
        public string AwardLevel { get; set; }
        public int JoinLevel { get; set; }
        public int JoinCount { get; set; }
        public int CreditHour { get; set; }
        public int AuditState { get; set; }
        public string AuditAccount { get; set; }
    }
}
